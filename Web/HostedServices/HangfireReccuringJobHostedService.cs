using Hangfire;

public class HangfireReccuringJobHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        RecurringJob.AddOrUpdate<RandomizerJob>(service => service.GetRandomDigit(), Cron.Minutely());
        RecurringJob.AddOrUpdate(() => Console.WriteLine(), Cron.Minutely());


        BackgroundJob.Enqueue<RandomizerJob>(service => service.GetRandomDigit());
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
