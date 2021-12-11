[LogEverything]
public class SampleJobs
{
    [LogEverything]
    public Task<long> GetRandomDigit()
    {
        return Task.FromResult(Random.Shared.NextInt64(0, 9));
    }
}
