namespace SHAgent
{
    public interface IProcessManager
    {
        void StartProcess(Action action);
        bool IsProcessRunning(Action action);
        string GetProcessOutput();
    }
}