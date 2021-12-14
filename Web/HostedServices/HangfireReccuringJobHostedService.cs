using Hangfire;

public class HangfireReccuringJobHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        RecurringJob.AddOrUpdate<RandomizerJob>(service => service.GetRandomDigit(null), Cron.Minutely());
        RecurringJob.AddOrUpdate(() => Console.WriteLine(), Cron.Minutely());


        BackgroundJob.Enqueue<RandomizerJob>(service => service.GetRandomDigit(null));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
