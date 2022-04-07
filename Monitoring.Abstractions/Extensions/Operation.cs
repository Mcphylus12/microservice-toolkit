using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Monitoring.Abstractions.Extensions;

public sealed class Operation<T> : IDisposable
{
    private readonly IMonitor<T> _monitor;
    private readonly string _name;
    private bool? _completed;
    private Exception? _exception;
    private readonly Stopwatch _timer;

    public Operation(IMonitor<T> monitor, string name)
    {
        _monitor = monitor;
        _name = name;

        _timer = Stopwatch.StartNew();
        _monitor.LogInformation("{OperationName} Started", name);
    }

    public void Succeed()
    {
        _completed = true;
    }

    public void Fail(Exception e)
    {
        _exception = e;
    }

    public void Dispose()
    {
        if (_exception is not null)
        {
            _monitor.LogError(_exception, "{OperationName} Failed in {elapsed}ms", _name, _timer.ElapsedMilliseconds);
            _monitor.Counter(_name, "fail").Increment();
            _monitor.Summary(_name, "fail").Submit(_timer.ElapsedMilliseconds);
        }
        else if (_completed.GetValueOrDefault() is true)
        {
            _monitor.LogInformation("{OperationName} Succeeded in {elapsed}ms", _name, _timer.ElapsedMilliseconds);
            _monitor.Counter(_name, "success").Increment();
            _monitor.Summary(_name, "success").Submit(_timer.ElapsedMilliseconds);
        }
        else
        {
            _monitor.LogInformation("{OperationName} Completed in {elapsed}ms", _name, _timer.ElapsedMilliseconds);
            _monitor.Counter(_name, "complete").Increment();
            _monitor.Summary(_name, "complete").Submit(_timer.ElapsedMilliseconds);
        }
    }
}
