using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monitoring.Abstractions;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace Monitoring;

public static class Registration
{
    public static void RegisterMonitoring(this IServiceCollection services)
    {
        services.AddTransient(typeof(IMonitor<>), typeof(Monitor<>));

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}")
            .CreateLogger();

        services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory());
    }
}
