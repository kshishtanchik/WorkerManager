using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.ServiceProcess;

namespace ServiceManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WinServiceController : ControllerBase
    {
        private readonly ILogger<WinServiceController> _logger;

        public WinServiceController(ILogger<WinServiceController> logger)
        {
            _logger = logger;
        }

        [HttpGet("AllServices")]
        public IActionResult AllServices()
        {
            var services = ServiceController.GetServices();

            return Ok(services.Select(s => new
            {
                name = s.ServiceName,
                status = s.Status.ToString("g"),
                startType = s.StartType.ToString("g"),
            }));
        }

        [HttpPost("ServiceInfo")]
        public IActionResult ServiceInfo(string serviceName)
        {

            var applicationLog = new EventLog("Application");
            var entries = applicationLog.Entries.Cast<EventLogEntry>();
            var logs = entries
                .Where(x => x.Source.Contains(serviceName))
                                    .Select(x => new
                                    {
                                        x.TimeGenerated,
                                        x.Site,
                                        x.Message
                                    })
                                    .ToList();
            return Ok(logs);
        }

        /// <summary>
        /// Запуск службы
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        [HttpPost("StartService")]
        public IActionResult StartService(string serviceName)
        {
            var service = new ServiceController(serviceName);
            // Проверяем не запущена ли служба
            if (service.Status != ServiceControllerStatus.Running)
            {
                // Запускаем службу
                service.Start();
                // В течении минуты ждём статус от службы
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(1));
                _logger.LogInformation("Служба была успешно запущена!");
            }
            else
            {
                _logger.LogWarning("Служба уже запущена!");
            }
            return Ok();
        }

        /// <summary>
        /// Останавливаем службу
        /// </summary>
        /// <param name="serviceName"></param>
        [HttpPost("StopService")]
        public IActionResult StopService(string serviceName)
        {
            var service = new ServiceController(serviceName);
            // Если служба не остановлена
            if (service.Status != ServiceControllerStatus.Stopped)
            {
                // Останавливаем службу
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(1));
                _logger.LogInformation("Служба была успешно остановлена!");
            }
            else
            {
                _logger.LogWarning("Служба уже остановлена!");
            }
            return Ok();
        }

        /// <summary>
        /// Перезапуск службы
        /// </summary>
        /// <param name="serviceName"></param>
        [HttpPost("RestartService")]
        public IActionResult RestartService(string serviceName)
        {
            var service = new ServiceController(serviceName);
            var timeout = TimeSpan.FromMinutes(1);
            if (service.Status != ServiceControllerStatus.Stopped)
            {
                Console.WriteLine("Перезапуск службы. Останавливаем службу...");
                // Останавливаем службу
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                _logger.LogInformation("Служба была успешно остановлена!");
            }
            if (service.Status != ServiceControllerStatus.Running)
            {
                Console.WriteLine("Перезапуск службы. Запускаем службу...");
                // Запускаем службу
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                _logger.LogInformation("Служба была успешно запущена!");
            }
            return Ok();
        }
    }
}
