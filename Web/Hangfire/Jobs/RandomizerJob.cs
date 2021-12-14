public class RandomizerJob
{
    private readonly ILogger<RandomizerJob> _logger;

    public RandomizerJob(ILogger<RandomizerJob> logger)
    {
        _logger = logger;
    }

    [LogEverything]
    public Task<long> GetRandomDigit()
    {
        return Task.FromResult(Random.Shared.NextInt64());
    }
}