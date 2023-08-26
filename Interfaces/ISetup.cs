namespace Interfaces.Setup
{
    public interface ISetup
    {
        void SetSourcePath(string sourcePath);
        void SetReplicaPath(string replicaPath);
        void SetLoggerPath(string loggerPath);
        void SetInterval(int interval);
    }
}