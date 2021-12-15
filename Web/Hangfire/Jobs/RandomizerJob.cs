using Hangfire.Server;
using Hangfire.Console;
using Hangfire.MissionControl;
using Hangfire;

[MissionLauncher(CategoryName = "SampleJobs")]
public class RandomizerJob
{
    private readonly ILogger<RandomizerJob> _logger;

    public RandomizerJob(ILogger<RandomizerJob> logger)
    {
        _logger = logger;
    }

    [LogEverything]
    [FailIfIntegerGreatherThan(10)]
    [DeleteSuceededAttribute]
    public async Task<long> GetRandomDigit(PerformContext? context = null)
    {
        _logger.LogInformation("Getting random digit");
        _logger.LogWarning("Getting random digit");
        context?.WriteLine("Getting random digit");
        var progressBar = context.WriteProgressBar("Name", 0);

        for (int i = 0; i < 10; i++)
        {
            progressBar.SetValue((i + 1) * 10);
            await Task.Delay(2000);
        }

        return Random.Shared.NextInt64(0, 20);
    }

    [Mission(Name = "GetRandomDigitMissionControlExample", Description = "gets digit (0-20)", Queue = "default")]
    public Task<long> GetRandomDigitMissionControlExample()
    {
        _logger.LogInformation("Getting random digit");
        _logger.LogWarning("Getting random digit");

        return Task.FromResult(Random.Shared.NextInt64(0, 20));
    }

    [Mission(Name = "GetRandomDigitMissionControlExample", Description = "gets digit (0-20)", Queue = "default")]
    public Task<long> GetRandomDigitMissionControlExample(long value)
    {
        _logger.LogInformation("Getting random digit");
        _logger.LogWarning("Getting random digit");

        return Task.FromResult(Random.Shared.NextInt64(value));
    }

    [Mission(Name = "FailIfAbove5", Description = "gets digit (0-10)", Queue = "default")]
    public Task<long> FailIfAbove5()
    {
        var value = Random.Shared.NextInt64(0, 10);

        if (value > 5)
            throw new ArgumentException($"Number is {value}");

        return Task.FromResult(value);
    }

    [Mission(Name = "Fail", Description = "FailI", Queue = "default")]
    public Task Fail()
    {
        throw new Exception();
    }
}