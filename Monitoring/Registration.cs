using Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monitoring.Abstractions;
using Prometheus;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Json;

namespace Monitoring;

public static class Registration
{
    public static void RegisterMonitoring(this IServiceCollection services, LogFormat format)
    {
        services.SetupOptions<MonitoringConfiguration>()
            .WithBinder<MonitoringConfigurationBinder>();

        services.AddTransient(typeof(IMonitor<>), typeof(Monitor<>));

        var config = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext();

        config = format switch
        {
            LogFormat.Console => config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}"),
            LogFormat.Json => config.WriteTo.Console(formatter: new JsonFormatter(renderMessage: true)),
            _ => throw new NotSupportedException("Console Format not supported")
        };

        Log.Logger = config.CreateLogger();

        services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory());
    }

    public static void StartMetrics(int port)
    {
        var metricServer = new KestrelMetricServer(port);
        metricServer.Start();
    }
}
