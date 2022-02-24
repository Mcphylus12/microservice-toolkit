using Communication;
using Communication.Abstractions.Registration;
using DemoShared;
using Monitoring;

namespace DemoWorker;

internal static class Registration
{
    internal static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterMonitoring();
        services.RegisterCommunication();
    }
}
