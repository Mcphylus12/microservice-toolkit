using Communication.Abstractions;
using DemoShared;
using Monitoring.Abstractions;

namespace DemoWorker;

public class DemoMessageHandler : IMessageHandler<DemoMessage>
{
    private readonly IMonitor<DemoMessageHandler> _monitor;

    public DemoMessageHandler(IMonitor<DemoMessageHandler> monitor)
    {
        this._monitor = monitor;
    }

    public Task HandleAsync(DemoMessage message, CancellationToken cancellationToken = default)
    {
        _monitor.LogInformation("Message Handled with value: {value}", message.Value);
        return Task.CompletedTask;
    }
}
