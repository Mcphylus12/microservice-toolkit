using Communication.Abstractions.Registration;
using Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monitoring.Abstractions;

namespace Communication.Abstractions.Internal;

internal class Sender : ISender
{
    private readonly Dictionary<string, string>? _requestConfig;
    private readonly Dictionary<string, string>? _messageConfig;
    private readonly IRequestImplementation _requestImplementation;
    private readonly IMessageImplementation _messageImplementation;

    public Sender(
        IOptions<CommunicationConfiguration> options,
        IMonitor<Sender> monitor,
        IRequestImplementation requestImplementation,
        IMessageImplementation messageImplementation)
    {
        _requestImplementation = requestImplementation;
        _messageImplementation = messageImplementation;
        if (options.Value.Outbound?.Requests?.Endpoints is null) monitor.LogInformation("Request Outbound config is null");
        if (options.Value.Outbound?.Messaging?.Endpoints is null) monitor.LogInformation("Messaging Outbound config is null");

        _requestConfig = options.Value.Outbound?.Requests?.Endpoints?
            .SelectMany(kv => kv.Value, (kv, requestType) => new { Key = kv.Key.Replace('#', ':'), requestType })
            .ToDictionary(o => o.requestType, o => o.Key);

        _messageConfig = options.Value.Outbound?.Messaging?.Endpoints?
            .SelectMany(kv => kv.Value, (kv, messageType) => new { Key = kv.Key.Replace('#', ':'), messageType })
            .ToDictionary(o => o.messageType, o => o.Key);
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        request.NotNull();
        _requestConfig.NotNull();

        var requestName = request.GetType().Name;
        if (_requestConfig!.TryGetValue(requestName, out var endpoint))
        {
            return _requestImplementation.SendRequest(endpoint, request);
        }
        else
        {
            throw new InvalidOperationException($"Endpoint not configured for request Type {requestName}");
        }
    }

    public Task Send(IRequest request)
    {
        request.NotNull();
        _requestConfig.NotNull();

        var requestName = request.GetType().Name;
        if (_requestConfig!.TryGetValue(requestName, out var endpoint))
        {
            return _requestImplementation.SendRequest(endpoint, request);
        }
        else
        {
            throw new InvalidOperationException($"Endpoint not configured for request Type {requestName}");
        }
    }

    public Task Send(IMessage message)
    {
        message.NotNull();
        _messageConfig.NotNull();

        var messageName = message.GetType().Name;
        if (_messageConfig!.TryGetValue(messageName, out var endpoint))
        {
            return _messageImplementation.SendMessage(endpoint, message);
        }
        else
        {
            throw new InvalidOperationException($"Endpoint not configured for message Type {messageName}");
        }
    }
}
