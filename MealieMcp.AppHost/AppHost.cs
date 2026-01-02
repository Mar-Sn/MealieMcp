var builder = DistributedApplication.CreateBuilder(args);

var mealie = builder
    .AddContainer("mealie", "hkotel/mealie")
    .WithHttpEndpoint(targetPort: 9000)
    .WithHttpHealthCheck("/api/app/about")
    .WithEnvironment("ALLOW_SIGNUP", "true")
    .WithEnvironment("INITIAL_ADMIN_EMAIL", "changeme@example.com")
    .WithEnvironment("INITIAL_ADMIN_PASSWORD", "MyPassword");

builder.AddProject<Projects.MealieMcp>("mealiemcp")
    .WithHttpEndpoint()
    .WithEnvironment("MEALIE_USERNAME", "changeme@example.com")
    .WithEnvironment("MEALIE_PASSWORD", "MyPassword")
    .WithEnvironment("MEALIE_API_URL", mealie.GetEndpoint("http"))
    .WaitFor(mealie);

builder.Build().Run();
