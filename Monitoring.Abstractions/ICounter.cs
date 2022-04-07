namespace Monitoring.Abstractions;

public interface ICounter
{
    void Increment();
    void Increment(int amount);
}
