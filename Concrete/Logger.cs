using Interfaces.Logger;

namespace Concrete.Logger
{
    public class Logger : ILogger
    {
        public void DisplayLogs()
        {
            Console.WriteLine("DisplayLogs works");
        }

        public void LogAction()
        {
            Console.WriteLine("LogAction works");
        }
    }
}