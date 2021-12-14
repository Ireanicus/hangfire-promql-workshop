using Hangfire.Client;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

public class LogEverythingFilter : IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
{
    private readonly ILogger _logger;

    public LogEverythingFilter(ILogger<LogEverythingFilter> logger)
    {
        _logger = logger;
    }

    public void OnCreating(CreatingContext context)
    {
        _logger.LogInformation("Creating a job based on method `{0}`...", context.Job.Method.Name);
    }

    public void OnCreated(CreatedContext context)
    {
        _logger.LogInformation(
            "Job that is based on method `{0}` has been created with id `{1}`",
            context.Job.Method.Name,
            context.BackgroundJob?.Id);
    }

    public void OnPerforming(PerformingContext context)
    {
        _logger.LogInformation("Starting to perform job `{0}`", context.BackgroundJob.Id);
    }

    public void OnPerformed(PerformedContext context)
    {
        _logger.LogInformation("Job `{0}` has been performed", context.BackgroundJob.Id);
    }

    public void OnStateElection(ElectStateContext context)
    {
        if (context.BackgroundJob.Job.Method.CustomAttributes
            .Any(attr => attr.AttributeType == typeof(LogEverythingAttribute)))
        {
            _logger.LogInformation("State election `{0}` -> '{1}'",
                context.CurrentState,
                context.CandidateState.Name);
        }

        var failedState = context.CandidateState as FailedState;
        if (failedState != null)
        {
            _logger.LogWarning(
                "Job `{0}` has been failed due to an exception `{1}`",
                context.BackgroundJob.Id,
                failedState.Exception);
        }
    }

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        _logger.LogInformation(
            "Job `{0}` state was changed from `{1}` to `{2}`",
            context.BackgroundJob.Id,
            context.OldStateName,
            context.NewState.Name);
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        _logger.LogInformation(
            "Job `{0}` state `{1}` was unapplied.",
            context.BackgroundJob.Id,
            context.OldStateName);
    }
}