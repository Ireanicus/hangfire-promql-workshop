using Hangfire.MissionControl;

[LogEverything]
[MissionLauncher(CategoryName = "SampleJobs")]
public class SampleJobs
{
    [LogEverything]
    [Mission(Name = "GetRandomDigit", Description = "gets digit (0-9)", Queue = "default")]
    public Task<long> GetRandomDigit()
    {
        return Task.FromResult(Random.Shared.NextInt64(0, 9));
    }

    [LogEverything]
    [Mission(Name = "GetDigit", Description = "gets digit (0-9)", Queue = "default")]
    public Task<long> GetRandomDigit(long digit)
    {
        return Task.FromResult(digit);
    }
}
