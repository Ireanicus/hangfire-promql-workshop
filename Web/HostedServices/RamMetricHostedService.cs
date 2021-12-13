using System.Diagnostics;
using App.Metrics;
using App.Metrics.Gauge;

public class RamMetricHostedService : IHostedService
{
    private readonly IMetrics _metrics;

    public RamMetricHostedService(IMetrics metrics)
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
        Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
        _metrics.Measure.Gauge.SetValue(new GaugeOptions
        {
            Name = "ram_used",
            MeasurementUnit = Unit.Bytes,
        }, currentProcess.WorkingSet64);


        long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;

        return Task.CompletedTask;
    }
}