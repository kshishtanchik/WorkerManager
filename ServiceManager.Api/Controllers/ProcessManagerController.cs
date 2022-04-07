using Microsoft.AspNetCore.Mvc;
using ServiceManager.Service.Abstractions;
using ServiceManager.Service.Models;
using System.Diagnostics;

namespace WebServiceController.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcessManagerController : ControllerBase
{
    private readonly ILogger<ProcessManagerController> _logger;
    private readonly IWorkerManager _workerManager;

    public ProcessManagerController(ILogger<ProcessManagerController> logger, IWorkerManager workerManager)
    {
        _logger = logger;
        _workerManager = workerManager;
    }
    /// <summary>
    /// Получить все запущенные процессы
    /// </summary>
    /// <returns></returns>
    [HttpGet("AllProcesses")]
    public IEnumerable<WorkerInfo> AlProcesses()
    {
        return _workerManager.AllWorkers();
    }

    [HttpPost("ProcessInfo")]
    public WorkerState ProcessInfo(string processName)
    {  
        return _workerManager.WorkerInfo(processName);
    }

    [HttpPost("KillProcess")]
    public WorkerState KillProcess(int processId)
    {
       return _workerManager.KillProcess(processId);        
    }

    [HttpPost("AddProcess")]
    public IActionResult AddProcess(int processId)
    {
        var process = Process.GetProcessById(processId);

        var processClone = new Process();
        processClone.StartInfo.FileName= process.MainModule.FileName;
        processClone.Start();
        return Ok($"Процесс id={processId} клонирован");
    }
}
