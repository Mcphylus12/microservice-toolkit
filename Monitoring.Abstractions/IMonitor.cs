using Microsoft.Extensions.Logging;

namespace Monitoring.Abstractions;

public interface IMonitor<out T> : ILogger<T>
{
    ICounter Counter(string name, params string[] labels);
    IGauge Gauge(string name, params string[] labels);
    ISummary Summary(string name, params string[] labels);
}
