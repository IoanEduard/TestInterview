using System.Diagnostics;
using task.Interfaces.Logger;
using task.models;

namespace task.Concrete.Logger
{
    public class Logger : ILogger
    {
        private Settings _settings;
        public Logger(Settings settings)
        {
            _settings = settings;
            InitializeLogFile();
        }

        public void DisplayLogs()
        {
            Process.Start("notepad.exe", @_settings.LoggerPath + "\\log.txt");
        }

        public void LogAction(string message)
        {
            var path = @_settings.LoggerPath + "\\log.txt";
            using (var sw = new StreamWriter(path, true))
            {
                sw.Write($"Log: {DateTime.Now} : {message}\n");
            }
        }

        private void InitializeLogFile()
        {
            var path = @_settings.LoggerPath + "\\log.txt";
            if (!File.Exists(path))
            {
                using (var sw = new StreamWriter(path, true))
                {
                    sw.Write($"Log file initialized at {DateTime.Now}!\n");
                }
            }
        }
    }
}