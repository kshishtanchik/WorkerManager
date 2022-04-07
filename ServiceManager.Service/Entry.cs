using Microsoft.Extensions.DependencyInjection;
using ServiceManager.Service.Abstractions;
using ServiceManager.Service.Services;

namespace ServiceManager.Service
{
    public static class Entry
    {
        public static IServiceCollection AddWorkerManager(this IServiceCollection services) {
            //todo: configuration
            return services.AddScoped<IWorkerManager, WorkerManager>();
        }
    }
}
