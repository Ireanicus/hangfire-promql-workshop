using App.Metrics;
using App.Metrics.Counter;
using Hangfire.States;
using Hangfire.Storage;

public class DurationHistogramFilter : IApplyStateFilter
{
    private readonly IMetrics _metrics;

    public DurationHistogramFilter(IMetrics metrics)
    {
        _metrics = metrics;
    }

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        if (context.NewState is SucceededState succeededState)
        {
            _metrics.Measure.Histogram.Update(new App.Metrics.Histogram.HistogramOptions
            {
                Name = "hangfire_job_duration",
                MeasurementUnit = Unit.None,
                Tags = new MetricTags(
                new[] { "method", "type" },
                new[] {
                    context.BackgroundJob.Job.Method.Name,
                    context.BackgroundJob.Job.Type.Name
            })
            }, succeededState.PerformanceDuration);
        }
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
    }
}