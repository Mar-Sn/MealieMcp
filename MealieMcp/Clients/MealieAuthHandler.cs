using System.Net.Http.Headers;
using MealieMcp.Client;

namespace MealieMcp.Clients;

public class MealieAuthHandler(IConfiguration config, ILogger<MealieAuthHandler> logger) : DelegatingHandler
{
    private string? _cachedToken;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _cachedToken ?? config["MEALIE_API_TOKEN"];

        if (string.IsNullOrEmpty(token))
        {
            var username = config["MEALIE_USERNAME"];
            var password = config["MEALIE_PASSWORD"];

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                token = await GetTokenViaLoginAsync(username, password, cancellationToken);
                _cachedToken = token;
            }
        }

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetTokenViaLoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        var baseUrl = config["MEALIE_API_URL"] ?? throw new InvalidOperationException("MEALIE_API_URL not set");
        client.BaseAddress = new Uri(baseUrl);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        });

        var response = await client.PostAsync("/api/auth/token", content, cancellationToken);
        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Failed to login to Mealie. Status: {response.StatusCode}, Content: {errorContent}");
        }

        logger.LogDebug("Login success. Content: {ErrorContent}", errorContent);

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken);
        return tokenResponse?.AccessToken ?? throw new InvalidOperationException("Failed to get access token from Mealie");
    }
}
