using Communication.Abstractions;
using DemoShared;
using Monitoring.Abstractions;
using Monitoring.Abstractions.Extensions;

namespace DemoWorker;

public class DemoRequestHandler : IRequestHandler<DemoRequest, DemoResponse>
{
    private readonly IMonitor<DemoRequestHandler> _monitor;

    public DemoRequestHandler(IMonitor<DemoRequestHandler> monitor)
    {
        this._monitor = monitor;
    }

    public async Task<DemoResponse> HandleAsync(DemoRequest request, CancellationToken cancellationToken = default)
    {
        using var operation = _monitor.StartOperation("demo_request_handler");
        _monitor.LogInformation("Request Handled with value: {value}", request.Value);
        await Task.Delay(1000);
        operation.Succeed();
        return new DemoResponse(request.Value);
    }
}
