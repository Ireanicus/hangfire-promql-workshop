using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

public class DeleteSuceededAttribute : JobFilterAttribute, IElectStateFilter
{
    public DeleteSuceededAttribute()
    {
    }

    public void OnStateElection(ElectStateContext context)
    {
        if (context.CandidateState is SucceededState succeededState)
        {
            context.CandidateState = new DeletedState
            {
                Reason = "Deleted because suceeded"
            };
        }
    }
}