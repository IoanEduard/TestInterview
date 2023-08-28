using Newtonsoft.Json;

namespace task.models
{
    public class Settings
    {
        public string SourcePath { get; set; }
        public string ReplicaPath { get; set; }
        public string LoggerPath { get; set; }
        public long Interval { get; set; }
        public string FilePath { get; } = @"assets/settings.json";

        public Settings()
        {
            if (File.Exists(FilePath)) {
                var content = File.ReadAllText(FilePath);
                JsonConvert.PopulateObject(content, this);
            }
            else {
                // create file
            }
        }
    }
}