using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MealieMcp.E2E;

public class McpServerFixture : IAsyncLifetime
{
    public DistributedApplication App { get; set; }
    private TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(5);

    public async Task InitializeAsync()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.MealieMcp_AppHost>(cancellationToken);
        appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
            logging.AddConsole(); // Add console logging to see output in test runner
        });
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        App = await appHost.BuildAsync(cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);
        await App.StartAsync(cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);
    }


    public async Task DisposeAsync()
    {
        await App.DisposeAsync();
    }
}