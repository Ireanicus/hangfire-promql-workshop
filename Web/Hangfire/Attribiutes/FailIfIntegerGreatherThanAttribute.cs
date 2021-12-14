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
        try
        {
            if (context.CandidateState is SucceededState succeededState)
            {
                if (succeededState.Result is long value && value > Value)
                    throw new ArgumentException($"Result is grather than {Value}");
            }
        }
        catch (Exception e)
        {
            // context.SetJobParameter<int>("RetryCount", int.MaxValue - 10);
            // context.CandidateState = new FailedState(e);

            context.CandidateState = new FailedNoRetryState(e.Message)
            {
                CustomProperty = $"Result is grather than {Value}"
            };
        }

    }
}

public class FailedNoRetryState : IState
{
    public FailedNoRetryState(string reason)
    {
        Reason = reason;
    }

    public string Name => nameof(FailedNoRetryState);

    public string Reason { get; }

    public string CustomProperty { get; set; } = "default";

    public bool IsFinal => true;

    public bool IgnoreJobLoadException => false;

    public Dictionary<string, string> SerializeData()
    {
        return new Dictionary<string, string> { { "key", "value" } };
    }
}
