using ExampleWorker;
using System.Diagnostics;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging => {
        logging.ClearProviders();
        logging.AddEventLog(cfg =>
        {
            cfg.SourceName = "worker";
        });
        logging.AddConsole();
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
