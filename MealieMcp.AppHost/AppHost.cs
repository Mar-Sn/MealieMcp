var builder = DistributedApplication.CreateBuilder(args);

var mealie = builder
    .AddContainer("mealie", "hkotel/mealie")
    .WithHttpEndpoint(targetPort: 9000);

builder.AddProject<Projects.MealieMcp>("mealiemcp")
    .WithHttpEndpoint()
    .WaitFor(mealie);

builder.Build().Run();
