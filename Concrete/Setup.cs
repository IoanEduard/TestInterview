using System.Text.RegularExpressions;
using enums;
using Interfaces.Logger;
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
        private ILogger _logger;
        private readonly string _windowsFilePathSystemPattern = @"^[a-zA-Z]:\\(((?![<>:""/\\|?*]).)+((?<![ .])\\)?)*$";
        public Setup(Settings settings, IDisplay display, ILogger logger)
        {
            _settings = settings;
            _display = display;
            _logger = logger;
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

            var userInput = ReadLine();
            var optionAsString = Enum.GetName(typeof(SetupOptions), option);

            switch (option)
            {
                case SetupOptions.SourcePath:
                    if (PathIsValid(userInput))
                        _settings.SourcePath = userInput;
                    break;
                case SetupOptions.ReplicaPath:
                    if (PathIsValid(userInput))
                        _settings.ReplicaPath = userInput;
                    break;
                case SetupOptions.LoggerPath:
                    if (PathIsValid(userInput))
                        _settings.LoggerPath = userInput;
                    break;
                case SetupOptions.Interval:
                    if (IntervalIsValid(userInput, out long milliseconds))
                        _settings.Interval = milliseconds;
                    break;
            }

            var content = File.ReadAllText(_settings.FilePath);
            var jsonObject = JObject.Parse(content);

            jsonObject[optionAsString] = userInput;
            var updatedFile = jsonObject.ToString();

            File.WriteAllText(_settings.FilePath, updatedFile);

            return true;

        }

        private bool PathIsValid(string userInput)
        {
            if (Regex.IsMatch(userInput, _windowsFilePathSystemPattern)) return true;
            else
            {
                _display.Show("Must be a valid absolute path");
                _logger.LogAction($"Tried to update an invalid path:\n the path was: {userInput}");

                return false;
            }
        }

        private bool IntervalIsValid(string userInput, out long milliseconds)
        {
            return long.TryParse(userInput, out milliseconds);
        }
    }
}