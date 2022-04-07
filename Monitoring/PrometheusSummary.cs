using Monitoring.Abstractions;

namespace Monitoring;

internal class PrometheusSummary : ISummary
{
    private readonly Prometheus.Histogram.Child _child;

    public PrometheusSummary(Prometheus.Histogram.Child child)
    {
        _child = child;
    }

    public void Submit(double value)
    {
        _child.Observe(value);
    }
}
