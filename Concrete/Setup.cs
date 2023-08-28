using System.Text.RegularExpressions;
using enums;
using Interfaces.Setup;
using Newtonsoft.Json.Linq;
using task.Interfaces;
using task.models;
using static System.Console;

namespace Concrete.Setup
{
    public class Setup : ISetup
    {
        private Settings _settings;
        private IDisplay _display;
        public Setup(Settings settings, IDisplay display)
        {
            _settings = settings;
            _display = display;
        }

        public bool SetInterval()
        {
            return UpdateJsonSettingsFile(SetupOptions.Interval);
        }

        public bool SetLoggerPath()
        {
            return UpdateJsonSettingsFile(SetupOptions.LoggerPath);
        }

        public bool SetReplicaPath()
        {
            return UpdateJsonSettingsFile(SetupOptions.ReplicaPath);
        }

        public bool SetSourcePath()
        {
            return UpdateJsonSettingsFile(SetupOptions.SourcePath);
        }

        private bool UpdateJsonSettingsFile(SetupOptions option)
        {
            _display.Show("Only absolute paths accepted\n");
            var value = ReadLine();
            var optionAsString = Enum.GetName(typeof(SetupOptions), option);

            var windowsFilePathSystemPattern = @"^[a-zA-Z]:\\(((?![<>:""/\\|?*]).)+((?<![ .])\\)?)*$";

            if (Regex.IsMatch(value, windowsFilePathSystemPattern))
            {
                switch (option)
                {
                    case SetupOptions.SourcePath:
                        _settings.SourcePath = value;
                        break;
                    case SetupOptions.ReplicaPath:
                        _settings.ReplicaPath = value;
                        break;
                    case SetupOptions.LoggerPath:
                        _settings.LoggerPath = value;
                        break;
                    case SetupOptions.Interval:
                        _settings.Interval = value;
                        break;
                }

                var content = File.ReadAllText(_settings.FilePath);
                var jsonObject = JObject.Parse(content);

                jsonObject[optionAsString] = value;
                var updatedFile = jsonObject.ToString();

                File.WriteAllText(_settings.FilePath, updatedFile);

                return true;
            }
            else
            {
                _display.Show("Must be a valid absolute path");
                return false;
            }

        }
    }
}