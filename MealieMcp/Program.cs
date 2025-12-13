using MealieMcp.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

builder.Services.AddHttpClient<MealieClient>()
    .ConfigureHttpClient((sp, client) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var token = config["MEALIE_API_TOKEN"];
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    })
    .AddTypedClient<MealieClient>((client, sp) =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var baseUrl = config["MEALIE_API_URL"] ?? "http://localhost:9000";
        return new MealieClient(baseUrl, client);
    });

builder.Services.AddTransient<MealieMcp.Tools.RecipeTools>();

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(Program).Assembly);

await builder.Build().RunAsync();