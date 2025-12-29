using Cake.Frosting;

return new CakeHost()
    .Run(args);

[TaskName("Default")]
public class DefaultTask : FrostingTask
{
}
