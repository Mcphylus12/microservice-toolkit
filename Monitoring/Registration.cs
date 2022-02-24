using Microsoft.Extensions.DependencyInjection;
using Monitoring.Abstractions;

namespace Monitoring;

public static class Registration
{
    public static void RegisterMonitoring(this IServiceCollection services)
    {
        services.AddTransient(typeof(IMonitor<>), typeof(Monitor<>));
    }
}
