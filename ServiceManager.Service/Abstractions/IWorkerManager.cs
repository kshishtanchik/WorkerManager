using ServiceManager.Service.Models;

namespace ServiceManager.Service.Abstractions
{
    public interface IWorkerManager
    {
        WorkerState AddProcess(int processId);
        IEnumerable<WorkerInfo> AllWorkers();
        WorkerState KillProcess(int processId);
        WorkerState WorkerInfo(string workerName);
    }
}