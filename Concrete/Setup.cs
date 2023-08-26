using Interfaces.Setup;
using static System.Console;

namespace Concrete.Setup
{
    public class Setup : ISetup
    {
        // fields and props with setter validation
        public void SetInterval(int interval)
        {
            WriteLine("SetInterval works");
        }

        public void SetLoggerPath(string loggerPath)
        {
            WriteLine("SetLoggerPath works");
        }

        public void SetReplicaPath(string replicaPath)
        {
            WriteLine("SetReplicaePath works");
        }

        public void SetSourcePath(string sourcePath)
        {
            WriteLine("SetSourcePath works");
        }
    }
}