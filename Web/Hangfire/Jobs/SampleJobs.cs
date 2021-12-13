using Hangfire.MissionControl;

[LogEverything]
[MissionLauncher(CategoryName = "SampleJobs")]
public class SampleJobs
{
    private readonly ILogger _logger;

    public SampleJobs(ILogger<SampleJobs> logger)
    {
        _logger = logger;
    }

    [LogEverything]
    [DeleteSuceeded]
    [CustomStatusExampleAttribute]
    [Mission(Name = "GetRandomDigit", Description = "gets digit (0-9)", Queue = "default")]
    public Task<long> GetRandomDigit()
    {
        _logger.LogWarning("GetRandom digit warning");
        _logger.LogError("GetRandom digit error");
        _logger.LogInformation("GetRandom digit information");
        return Task.FromResult(Random.Shared.NextInt64(0, 9));
    }

    [LogEverything]
    [Mission(Name = "GetDigit", Description = "gets digit (0-9)", Queue = "default")]
    public Task<long> GetRandomDigit(long digit)
    {
        return Task.FromResult(digit);
    }
}
