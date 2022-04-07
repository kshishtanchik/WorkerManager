using Microsoft.Extensions.Logging;
using ServiceManager.Service.Abstractions;
using ServiceManager.Service.Models;
using System.Diagnostics;

namespace ServiceManager.Service.Services
{
    public class WorkerManager : IWorkerManager
    {
        private readonly ILogger<WorkerManager> _logger;

        public WorkerManager(ILogger<WorkerManager> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Получить все запущенные процессы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WorkerInfo> AllWorkers()
        {
            var pocesses = Process.GetProcesses();

            return pocesses.Select(s => new WorkerInfo
            {
                Id = s.Id,
                Name = s.ProcessName
            });
        }

        public WorkerState WorkerInfo(string workerName)
        {
            var workers = Process.GetProcessesByName(workerName);
            var info = workers.Select(x => new WorkerInfo { Id = x.Id, TotalProcessorTime = x.TotalProcessorTime });
            return new WorkerState { Info = info };
        }

        public WorkerState KillProcess(int processId)
        {
            var worker = Process.GetProcessById(processId);

            worker.Kill();
            return new WorkerState { State = "kill" };
        }

        public WorkerState AddProcess(int processId)
        {
            var process = Process.GetProcessById(processId);

            var processClone = new Process();
            processClone.StartInfo.FileName = process.MainModule.FileName;
            processClone.Start();
            return WorkerInfo(process.ProcessName);
        }
    }
}