using App.Metrics;
using App.Metrics.Gauge;
using Hangfire;

public class HangfireDashboardMetricsHostedService : IHostedService
{
    private readonly IMetrics _metrics;

    public HangfireDashboardMetricsHostedService(IMetrics metrics)
    {
        _metrics = metrics;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        System.Timers.Timer timer = new System.Timers.Timer(10000);
        timer.Elapsed += async (sender, e) => await HandleTimer();
        timer.Start();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private Task HandleTimer()
    {
        var monitoringApi = JobStorage.Current.GetMonitoringApi();
        var statistics = monitoringApi.GetStatistics();

        foreach (var property in statistics
            .GetType().GetProperties())
        {
            _metrics.Measure.Gauge.SetValue(new GaugeOptions
            {
                Name = $"hangfire_stats_{property.Name}",
                MeasurementUnit = Unit.None,
            }, (long)property.GetValue(statistics));
        }

        return Task.CompletedTask;
    }
}