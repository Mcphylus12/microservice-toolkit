using Communication;
using Monitoring;
using Monitoring.Abstractions;

namespace DemoApi;

internal static class Registration
{
    internal static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterMonitoring(LogFormat.Console);
        services.RegisterCommunication();
    }
}
