using Prometheus;

namespace Monitoring;

internal class PrometheusCounter : Abstractions.ICounter
{
    private readonly Counter.Child _child;

    public PrometheusCounter(Counter.Child child)
    {
        _child = child;
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
