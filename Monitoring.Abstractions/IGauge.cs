namespace Monitoring.Abstractions;

public interface IGauge
{
    void Increment();
    void Increment(int amount);

    void Decrement();
    void Decrement(int amount);

    void Force(int newValue);
}
