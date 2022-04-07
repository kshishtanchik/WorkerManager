using Microsoft.AspNetCore.Mvc;
using ServiceManager.Service.Abstractions;
using ServiceManager.Service.Models;
using System.Diagnostics;

namespace WebServiceController.Controllers;

/// <summary>
/// Менеджер запущенных воркеров
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProcessManagerController : ControllerBase
{
    private readonly ILogger<ProcessManagerController> _logger;
    private readonly IWorkerManager _workerManager;


    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="workerManager"></param>
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

    /// <summary>
    /// Информация по имени воркера
    /// </summary>
    /// <param name="processName"> имя воркера</param>
    /// <returns></returns>
    [HttpPost("ProcessInfo")]
    public WorkerState ProcessInfo(string processName)
    {  
        return _workerManager.WorkerInfo(processName);
    }

    /// <summary>
    /// Убить процес
    /// </summary>
    /// <param name="processId">идентификатор процесса</param>
    /// <returns></returns>
    [HttpPost("KillProcess")]
    public WorkerState KillProcess(int processId)
    {
       return _workerManager.KillProcess(processId);        
    }

    /// <summary>
    /// Добавить экземпляр воркера
    /// </summary>
    /// <param name="workerName"></param>
    /// <returns></returns>
    [HttpPost("AddProcess")]
    public IActionResult AddProcess(string workerName)
    {
        var worker = Process.GetProcessesByName(workerName).FirstOrDefault();

        var processClone = new Process();
        processClone.StartInfo.FileName= worker.MainModule.FileName;
        processClone.Start();
        return Ok($"Процесс id={workerName} клонирован");
    }
}
