using Communication;
using Communication.Abstractions.Registration;
using DemoShared;
using Monitoring;
using Monitoring.Abstractions;

namespace DemoWorker;

internal static class Registration
{
    internal static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterMonitoring(LogFormat.Console);
        services.RegisterCommunication();
    }
}
