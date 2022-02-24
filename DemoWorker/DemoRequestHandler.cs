using Communication.Abstractions;
using DemoShared;
using Monitoring.Abstractions;

namespace DemoWorker;

public class DemoRequestHandler : IRequestHandler<DemoRequest, DemoResponse>
{
    private readonly IMonitor<DemoRequestHandler> _monitor;

    public DemoRequestHandler(IMonitor<DemoRequestHandler> monitor)
    {
        this._monitor = monitor;
    }

    public Task<DemoResponse> HandleAsync(DemoRequest request, CancellationToken cancellationToken = default)
    {
        _monitor.LogInformation("Request Handled with value: {value}", request.Value);
        return Task.FromResult(new DemoResponse(request.Value));
    }
}
