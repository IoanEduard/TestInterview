namespace Interfaces.Setup
{
    public interface ISetup
    {
        bool SetSourcePath();
        bool SetReplicaPath();
        bool SetLoggerPath();
        bool SetInterval();
    }
}