using MealieMcp.Clients;
using MealieMcp.Client;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Refit;
using Polly;
using Polly.Extensions.Http;

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
            .AddAspNetCoreInstrumentation();
    });

var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

var jsonOptions = new System.Text.Json.JsonSerializerOptions
{
    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
};

var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions)
};

builder.Services.AddRefitClient<IMealieClient>(refitSettings)
    .ConfigureHttpClient((sp, client) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var baseUrl = config["MEALIE_API_URL"] ?? throw new InvalidOperationException("MEALIE_API_URL not set");
        client.BaseAddress = new Uri(baseUrl);
    })
    .AddHttpMessageHandler(sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var token = config["MEALIE_API_TOKEN"] ?? throw new InvalidOperationException("MEALIE_API_TOKEN not set");;
        return new MealieAuthHandler(token);
    })
    .AddHttpMessageHandler<LoggingHandler>()
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddTransient<LoggingHandler>();
builder.Services.AddTransient<MealieMcp.Tools.RecipeTools>();
builder.Services.AddTransient<MealieMcp.Tools.FoodTools>();
builder.Services.AddTransient<MealieMcp.Tools.TagTools>();

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
