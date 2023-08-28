
namespace task.Interfaces.Synchronizer
{
    public interface ISynchronizer
    {
        Task ExecuteScheduledSynchronizationAsync();
        void ManualSync();
    }
}