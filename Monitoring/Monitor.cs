using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monitoring.Abstractions;
using Prometheus;

namespace Monitoring;

internal class Monitor<T> : IMonitor<T>
{
    private readonly ConcurrentDictionary<string, Abstractions.ICounter> _counters;
    private readonly ConcurrentDictionary<string, Abstractions.ISummary> _summaries;
    private readonly ConcurrentDictionary<string, Abstractions.IGauge> _gauges;
    private readonly ILogger<T> _logger;
    private readonly MonitoringConfiguration _options;

    public Monitor(ILogger<T> logger, IOptions<MonitoringConfiguration> options)
    {
        _logger = logger;
        _options = options.Value;
        _counters = new ConcurrentDictionary<string, Abstractions.ICounter>();
        _gauges = new ConcurrentDictionary<string, Abstractions.IGauge>();
        _summaries = new ConcurrentDictionary<string, Abstractions.ISummary>();
    }

    public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope(state);

    public Abstractions.ICounter Counter(string name, params string[] labels)
    {
        var key = string.Join('-', name, labels);

        return _counters.GetOrAdd(key, k =>
        {
            return new PrometheusCounter(Metrics.CreateCounter(name + "_Counter", "help", new CounterConfiguration
            {
                LabelNames = labels.Select((l, i) => $"L{i}").ToArray()
            }).WithLabels(labels));
        });
    }

    public Abstractions.IGauge Gauge(string name, params string[] labels)
    {
        var key = string.Join('-', name, labels);

        return _gauges.GetOrAdd(key, k =>
        {
            return new PrometheusGauge(Metrics.CreateGauge(name + "_Gauge", "help", new GaugeConfiguration
            {
                LabelNames = labels.Select((l, i) => $"L{i}").ToArray()
            }).WithLabels(labels));
        });
    }

    public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => _logger.Log(logLevel, eventId, state, exception, formatter);

    public Abstractions.ISummary Summary(string name, params string[] labels)
    {
        var key = string.Join('-', name, labels);

        return _summaries.GetOrAdd(key, k =>
        {
            return new PrometheusSummary(Metrics.CreateHistogram(name + "_Histogram", "help", new HistogramConfiguration
            {
                LabelNames = labels.Select((l, i) => $"L{i}").ToArray(),
                Buckets = _options.HistogramBins.ContainsKey(name) ? _options.HistogramBins[name].ToArray() : null
            }).WithLabels(labels));
        });
    }
}
