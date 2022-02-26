using Communication.Abstractions.Registration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Monitoring.Abstractions;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Communication;

internal class RequestMiddleware : IMiddleware
{
    private readonly IMonitor<RequestMiddleware> _monitor;
    private readonly IResolver _requestResolver;

    public RequestMiddleware(
        IMonitor<RequestMiddleware> monitor,
        IResolver requestResolver)
    {
        _monitor = monitor;
        _requestResolver = requestResolver;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.Any(header => header.Key == "X-Request-Type"))
        {
            _monitor.LogDebug("Skipping Request Handler as header Not present");
            await next(context);
            return;
        }

        var messageType = context.Request.Headers["X-Request-Type"];
        using var scope = _monitor.BeginScope(new Dictionary<string, object>
        {
            ["RequestHandlingId"] = Guid.NewGuid(),
            ["MessageType"] = messageType.Single()
        });
        var response = await _requestResolver.Resolve(messageType, async type =>
        {
            var request = await context.Request.ReadFromJsonAsync(type);

            if (request is null) throw new JsonSerializationException();

            return request;
        });

        if (response is not null)
        {
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
