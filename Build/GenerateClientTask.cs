using Cake.Common;
using Cake.Core.IO;

[TaskName("Generate-Client")]
[IsDependentOn(typeof(PatchOpenApiTask))]
public class GenerateClientTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Cleaning client directory...");
        context.CleanDirectory(context.ClientProjectDir, (file) => !file.Path.FullPath.EndsWith(".csproj"));

        context.Log.Information("Generating Kiota client...");
        var exitCode = context.StartProcess("dotnet", new ProcessSettings
        {
            Arguments = new ProcessArgumentBuilder()
                .Append("kiota")
                .Append("generate")
                .Append("--openapi")
                .AppendQuoted(context.ProcessedOpenApiSpecFile)
                .Append("--language")
                .Append("CSharp")
                .Append("--class-name")
                .Append("MealieClient")
                .Append("--namespace-name")
                .Append("MealieMcp.Clients") 
                .Append("--output")
                .AppendQuoted(context.ClientProjectDir)
        });

        if (exitCode != 0)
        {
            throw new Exception("Kiota generation failed.");
        }
    }
}