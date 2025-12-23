using MealieMcp.Clients;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

var builder = WebApplication.CreateBuilder(args);

var serviceName = "MealieMcp";

builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
    logging.ParseStateValues = true;
    logging.AddOtlpExporter();
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: serviceName))
    .WithTracing(tracing =>
    {
        tracing
            .AddSource(serviceName)
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter();
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddRuntimeInstrumentation()
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter();
    });

builder.Services.AddHttpClient<MealieClient>()
    .ConfigureHttpClient((sp, client) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var baseUrl = config["MEALIE_API_URL"] ?? throw new InvalidOperationException("MEALIE_API_URL not set");
        client.BaseAddress = new Uri(baseUrl);
    })
    .AddTypedClient<MealieClient>((client, sp) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var token = config["MEALIE_API_TOKEN"];
        
        IAuthenticationProvider authProvider;
        if (!string.IsNullOrEmpty(token))
        {
             // Use custom provider to allow HTTP (localhost)
             authProvider = new InsecureApiKeyAuthenticationProvider($"Bearer {token}", "Authorization", InsecureApiKeyAuthenticationProvider.KeyLocation.Header);
        }
        else
        {
             authProvider = new AnonymousAuthenticationProvider();
        }

        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: client);
        adapter.BaseUrl = client.BaseAddress?.ToString();
        return new MealieClient(adapter);
    });

builder.Services.AddTransient<MealieMcp.Tools.RecipeTools>();
builder.Services.AddTransient<MealieMcp.Tools.FoodTools>();

var mcpBuilder = builder.Services.AddMcpServer()
    .WithToolsFromAssembly(typeof(Program).Assembly);

// Determine transport mode
var useStdio = args.Contains("--stdio") || builder.Configuration["MCP_TRANSPORT"]?.ToLower() == "stdio";

if (useStdio)
{
    mcpBuilder.WithStdioServerTransport();
}
else
{
    mcpBuilder.WithHttpTransport();
}

var app = builder.Build();

if (!useStdio)
{
    var apiKey = builder.Configuration["MCP_SERVER_API_KEY"];
    if (!string.IsNullOrEmpty(apiKey))
    {
        app.UseWhen(context => context.Request.Path.StartsWithSegments("/mcp"), appBuilder =>
        {
            appBuilder.UseMiddleware<MealieMcp.Middleware.ApiKeyAuthMiddleware>(apiKey);
        });
    }

    // Configure the HTTP request pipeline for SSE
    app.MapMcp("/mcp");
}

await app.RunAsync();
