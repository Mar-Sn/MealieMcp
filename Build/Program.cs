using Cake.Git;

return new CakeHost()
    .UseContext<BuildContext>()
    .Run(args);

public class BuildContext : FrostingContext
{
    private string SolutionDir { get; }
    public string OpenApiJson { get; }
    public string ClientProjectDir { get; }

    public string ProcessedOpenApiSpecFile { get; } = "openapi.processed.json";

    public BuildContext(ICakeContext context) : base(context)
    {
        SolutionDir = context.GitFindRootFromPath(context.Environment.WorkingDirectory).FullPath;
        OpenApiJson = Path.Combine(SolutionDir, "Build", "OpenApiSpec", "openapi.json");
        ClientProjectDir = Path.Combine(SolutionDir, "MealieMcp.Client");
    }
}


[TaskName("Default")]
[IsDependentOn(typeof(GenerateClientTask))]
public class DefaultTask : FrostingTask
{
}