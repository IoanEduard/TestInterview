
namespace Interfaces.Synchronizer
{
    public interface ISynchronizer
    {
        Task ExecuteScheduledAsync();
        void ManualSync();
    }
}