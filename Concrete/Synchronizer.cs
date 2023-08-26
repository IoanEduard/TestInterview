
using Interfaces.Synchronizer;

namespace Concrete.Synchronizer
{
    public class Synchronizer : ISynchronizer
    {
        public void SyncNow()
        {
            Console.WriteLine("IT WORKS");
        }
    }
}