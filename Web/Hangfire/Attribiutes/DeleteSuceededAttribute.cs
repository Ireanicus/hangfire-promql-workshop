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
        var succeededState = context.CandidateState as SucceededState;
        if (succeededState != null)
        {
            context.CandidateState = new DeletedState
            {
                Reason = "Deleted because suceeded"
            };
        }
    }
}