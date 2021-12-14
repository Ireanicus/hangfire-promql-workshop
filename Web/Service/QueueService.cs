public class QueueService
{
    public string GetQueue(long id)
    {
        switch(id)
        {
            case 0: return "mz";
            default: return "bz";
        }
    }
}