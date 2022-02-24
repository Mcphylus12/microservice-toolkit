using Communication;
using Monitoring;

namespace DemoApi;

internal static class Registration
{
    internal static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterMonitoring();
        services.RegisterCommunication();
    }
}
