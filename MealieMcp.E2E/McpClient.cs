using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace MealieMcp.E2E;

public class McpClient(HttpClient httpClient) : IAsyncDisposable
{
    private string? _sessionId;
    private string? _messageEndpoint;
    private Task? _sseTask;
    private readonly CancellationTokenSource _cts = new();
    private readonly System.Collections.Concurrent.ConcurrentDictionary<string, TaskCompletionSource<JsonNode?>> _pendingRequests = new();

    public async Task InitializeAsync(string mcpEndpoint = "mcp", CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, mcpEndpoint);
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));
        
        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var reader = new StreamReader(stream);

        // We need to find the 'endpoint' event. It might not be the first line.
        while (string.IsNullOrEmpty(_messageEndpoint) && !_cts.Token.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (line == null) break;

            if (line.StartsWith("event: endpoint"))
            {
                var dataLine = await reader.ReadLineAsync(cancellationToken);
                if (dataLine != null && dataLine.StartsWith("data: "))
                {
                    var url = dataLine["data: ".Length..];
                    var uri = new Uri(url, UriKind.RelativeOrAbsolute);
                    if (!uri.IsAbsoluteUri)
                    {
                        uri = new Uri(httpClient.BaseAddress!, uri);
                    }
                    
                    _messageEndpoint = uri.ToString();
                    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    _sessionId = query["sessionId"];
                }
            }
        }

        if (string.IsNullOrEmpty(_sessionId))
        {
            throw new Exception("Failed to establish MCP session: sessionId not found in SSE stream.");
        }

        // Keep the SSE stream open in the background to maintain the session and receive responses
        _sseTask = Task.Run(async () =>
        {
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    var line = await reader.ReadLineAsync(_cts.Token);
                    if (line == null) break;

                    if (line.StartsWith("event: message"))
                    {
                        var dataLine = await reader.ReadLineAsync(_cts.Token);
                        if (dataLine != null && dataLine.StartsWith("data: "))
                        {
                            var data = dataLine["data: ".Length..];
                            // Parse as JsonNode to find the ID, but we might want to preserve the original structure
                            // for the actual result to avoid JsonNode weirdness if possible
                            using var doc = System.Text.Json.JsonDocument.Parse(data);
                            var id = doc.RootElement.TryGetProperty("id", out var idProp) ? idProp.GetString() : null;
                            if (id != null && _pendingRequests.TryRemove(id, out var tcs))
                            {
                                // In SSE transport, the data is the whole JSON-RPC response object
                                // We parse it back to JsonNode for the result
                                tcs.SetResult(JsonNode.Parse(data));
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"SSE stream error: {ex.Message}");
                foreach (var tcs in _pendingRequests.Values)
                {
                    tcs.TrySetException(ex);
                }
            }
        }, _cts.Token);
    }

    public async Task<JsonNode?> PostMessageAsync(string method, object? @params = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_messageEndpoint))
        {
            throw new InvalidOperationException("McpClient is not initialized.");
        }

        var id = Guid.NewGuid().ToString();
        var tcs = new TaskCompletionSource<JsonNode?>();
        _pendingRequests[id] = tcs;

        var payload = new
        {
            jsonrpc = "2.0",
            method = method,
            @params = @params ?? new { },
            id = id
        };

        var requestBody = System.Text.Json.JsonSerializer.Serialize(payload);
        Console.WriteLine($"[DEBUG_LOG] MCP POST Request to {_messageEndpoint}: {requestBody}");

        var response = await httpClient.PostAsJsonAsync(_messageEndpoint, payload, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _pendingRequests.TryRemove(id, out _);
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"MCP POST failed with {response.StatusCode}: {errorContent}");
        }

        // Wait for the response from SSE
        var responseJson = await tcs.Task.WaitAsync(TimeSpan.FromMinutes(5), cancellationToken);
        
        if (responseJson?["error"] != null)
        {
            var error = responseJson["error"];
            var message = error?["message"]?.GetValue<string>() ?? "Unknown error";
            var data = error?["data"]?.ToString();
            throw new Exception($"MCP JSON-RPC Error: {message} {(data != null ? $" - Data: {data}" : "")}");
        }

        return responseJson?["result"]?.DeepClone();
    }

    public async Task<JsonNode?> CallToolAsync(string name, object? arguments = null, CancellationToken cancellationToken = default)
    {
        return await PostMessageAsync("tools/call", new { name, arguments }, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        _cts.Cancel();
        if (_sseTask != null)
        {
            try
            {
                await _sseTask;
            }
            catch { }
        }
        _cts.Dispose();
        httpClient.Dispose();
    }
}
