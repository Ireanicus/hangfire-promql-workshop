using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

public class CustomStatusExampleAttribute : JobFilterAttribute, IElectStateFilter
{
    public CustomStatusExampleAttribute()
    {
    }

    public void OnStateElection(ElectStateContext context)
    {
        var deletedState = context.CandidateState as DeletedState;
        if (deletedState != null)
        {
            context.CandidateState = new ExampleState();
        }
    }

    public class ExampleState : IState
    {
        public string Name => "Example";

        public string Reason => "Just and example";

        public bool IsFinal => true;

        public bool IgnoreJobLoadException => false;

        public Dictionary<string, string> SerializeData()
        {
            return new Dictionary<string, string>();
        }
    }
}