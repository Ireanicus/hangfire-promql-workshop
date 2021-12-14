using Hangfire.MissionControl;

[MissionLauncher(CategoryName = "Queue")]
public class MultiQueueExample
{
    [CustomQueue]
    [Mission(Name = "Queue", Description = "example", Queue = "default")]
    public Task Execute(long id)
    {
        return Task.CompletedTask;
    }
}