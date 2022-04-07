namespace Monitoring.Abstractions;

public interface ISummary
{
    void Submit(double value);
}
