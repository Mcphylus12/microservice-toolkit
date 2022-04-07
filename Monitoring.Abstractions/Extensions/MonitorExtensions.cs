namespace Monitoring.Abstractions.Extensions;
public static class MonitorExtensions
{
    public static Operation<T> StartOperation<T>(this IMonitor<T> monitor, string name)
    {
        return new Operation<T>(monitor, name);
    }
}
