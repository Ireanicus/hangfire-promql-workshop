using System.Reflection;
using App.Metrics;
using App.Metrics.Gauge;

public class LabelMetricHostedService : IHostedService
{
    private readonly IMetrics _metrics;

    public LabelMetricHostedService(IMetrics metrics)
    {
        _metrics = metrics;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _metrics.Measure.Gauge.SetValue(new GaugeOptions
        {
            Name = "label",
            Tags = new MetricTags("version", Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString())
        }, 1);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
