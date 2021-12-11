using Hangfire;


public class RecurringJobsHostedService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        RecurringJob.AddOrUpdate<SampleJobs>(jobs => jobs.GetRandomDigit(), Cron.Minutely());

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
