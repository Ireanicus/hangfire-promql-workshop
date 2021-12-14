using Hangfire.States;

public class CustomQueueFilter : IElectStateFilter
{
    private readonly QueueService _service;

    public CustomQueueFilter(QueueService service)
    {
        _service = service;
    }

    public void OnStateElection(ElectStateContext context)
    {
        if (context.CandidateState is EnqueuedState enqueuedState
            && context.BackgroundJob.Job.Method.CustomAttributes
                .Any(attr => attr.AttributeType == typeof(CustomQueueAttribute)))
        {
            var id = (long)context.BackgroundJob.Job.Args[0];

            enqueuedState.Queue = _service.GetQueue(id);
        }
    }
}