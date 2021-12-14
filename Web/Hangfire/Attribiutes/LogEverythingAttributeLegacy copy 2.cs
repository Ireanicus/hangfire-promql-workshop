using Hangfire.Common;
using Hangfire.States;

public class FailIfIntegerGreatherThanAttribute : JobFilterAttribute, IElectStateFilter
{
    public FailIfIntegerGreatherThanAttribute(int value)
    {
        Value = value;
    }

    public long Value { get; }

    public void OnStateElection(ElectStateContext context)
    {
        if (context.CandidateState is SucceededState succeededState)
        {
            if (succeededState.Result is long value && value > Value)
                throw new ArgumentException($"Result is grather than {Value}");
        }
    }
}