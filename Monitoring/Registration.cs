using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monitoring.Abstractions;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Json;

namespace Monitoring;

public static class Registration
{
    public static void RegisterMonitoring(this IServiceCollection services, ConsoleFormat format)
    {
        services.AddTransient(typeof(IMonitor<>), typeof(Monitor<>));

        var config = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext();

        config = format switch
        {
            ConsoleFormat.Console => config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{Properties:j}{Exception}"),
            ConsoleFormat.Json => config.WriteTo.Console(formatter: new JsonFormatter(renderMessage: true)),
            _ => throw new NotSupportedException("Console Format not supported")
        };

        Log.Logger = config.CreateLogger();

        services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory());
    }
}
