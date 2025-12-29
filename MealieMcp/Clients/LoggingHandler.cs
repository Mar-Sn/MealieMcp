namespace MealieMcp.Clients;

public class LoggingHandler(ILogger<LoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content != null)
        {
            var content = await request.Content.ReadAsStringAsync(cancellationToken);
            logger.LogInformation("Request Body: {Content}", content);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
