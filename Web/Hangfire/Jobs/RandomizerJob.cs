public class RandomizerJob
{
    public Task<long> GetRandomDigit()
    {
        return Task.FromResult(Random.Shared.NextInt64());
    }
}