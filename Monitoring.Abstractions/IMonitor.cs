using Microsoft.Extensions.Logging;

namespace Monitoring.Abstractions;

public interface IMonitor<out T> : ILogger<T>
{
}
