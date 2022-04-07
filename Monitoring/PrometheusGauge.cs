using Monitoring.Abstractions;

namespace Monitoring;

internal class PrometheusGauge : IGauge
{
    private readonly Prometheus.Gauge.Child _child;

    public PrometheusGauge(Prometheus.Gauge.Child child)
    {
        _child = child;
    }

    public void Decrement()
    {
        _child.Dec();
    }

    public void Decrement(int amount)
    {
        _child.Dec(amount);
    }

    public void Force(int newValue)
    {
        _child.Set(newValue);
    }

    public void Increment()
    {
        _child.Inc();
    }

    public void Increment(int amount)
    {
        _child.Inc(amount);
    }
}
