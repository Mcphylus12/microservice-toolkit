using Microsoft.Extensions.Logging;
using Monitoring.Abstractions;

namespace Monitoring;

internal class Monitor<T> : IMonitor<T>
{
    private readonly ILogger<T> _logger;

    public Monitor(ILogger<T> logger)
    {
        _logger = logger;
    }

    public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state);

    public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => _logger.Log(logLevel, eventId, state, exception, formatter);
}
