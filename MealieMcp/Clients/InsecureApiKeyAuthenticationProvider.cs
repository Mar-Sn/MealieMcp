using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace MealieMcp.Clients;

public class InsecureApiKeyAuthenticationProvider(
    string apiKey,
    string parameterName,
    InsecureApiKeyAuthenticationProvider.KeyLocation keyLocation)
    : IAuthenticationProvider
{
    public enum KeyLocation
    {
        QueryParameter,
        Header
    }

    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
        if(request == null) throw new ArgumentNullException(nameof(request));

        switch(keyLocation)
        {
            case KeyLocation.QueryParameter:
                var uri = request.URI;
                // Simplified for brevity, typically would use UriBuilder
                if (!request.QueryParameters.ContainsKey(parameterName))
                {
                    // This part is tricky with Kiota's RequestInformation, usually we just add to query params if supported
                    // But for header it's easier.
                    // For now, we only need Header support for Mealie.
                }
                break;
            case KeyLocation.Header:
                if (!request.Headers.ContainsKey(parameterName))
                {
                    request.Headers.Add(parameterName, apiKey);
                }
                break;
        }

        return Task.CompletedTask;
    }
}
