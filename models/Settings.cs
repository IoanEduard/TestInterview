using Newtonsoft.Json;

namespace task.models
{
    public class Settings
    {
        public string? SourcePath { get; set; }
        public string? ReplicaPath { get; set; }
        public string? LoggerPath { get; set; }
        public string? Interval { get; set; }

        public Settings()
        {
            var path = "assets/settings.json";
            if (File.Exists(path)) {
                var content = File.ReadAllText(path);
                JsonConvert.PopulateObject(content, this);
            }
        }
    }
}