using App.Metrics;
using App.Metrics.Counter;
using Hangfire.States;
using Hangfire.Storage;

public class StateMetricFilter : IElectStateFilter, IApplyStateFilter
{
    private readonly IMetrics _metrics;

    public StateMetricFilter(IMetrics metrics)
    {
        _metrics = metrics;
    }

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        MeterStateChange("on_state_applied", context);
    }
    public void OnStateElection(ElectStateContext context)
    {
        _metrics.Measure.Counter.Increment(new CounterOptions
        {
            Name = "on_state_election",
            MeasurementUnit = Unit.None,
            Tags = new MetricTags(new[] { "method", "state", "candidateState", "type" },
                new[]{
                    context.BackgroundJob.Job.Method.Name,
                    context.CurrentState ?? "initial",
                    context.CandidateState.Name,
                    context.BackgroundJob.Job.Type.Name
                })
        });
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        MeterStateChange("on_state_unapplied", context);
    }

    private void MeterStateChange(string name, ApplyStateContext context)
    {
        _metrics.Measure.Counter.Increment(new CounterOptions
        {
            Name = name,
            MeasurementUnit = Unit.None,
            Tags = new MetricTags(
                new[] { "method", "type", "newState", "oldState" },
                new[] {
                    context.BackgroundJob.Job.Method.Name,
                    context.BackgroundJob.Job.Type.Name,
                    context.NewState.Name,
                    context.OldStateName ?? "initial"
            })
        });
    }
}