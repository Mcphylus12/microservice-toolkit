using Communication.Abstractions;
using Communication.Abstractions.Registration;
using Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monitoring.Abstractions;

namespace Communication;

internal class Sender : ISender
{
    private readonly IRequestImplementation _requestImplementation;
    private readonly IMessageImplementation _messageImplementation;

    public Sender(
        IRequestImplementation requestImplementation,
        IMessageImplementation messageImplementation)
    {
        _requestImplementation = requestImplementation;
        _messageImplementation = messageImplementation;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        request.NotNull();
        return _requestImplementation.SendRequest(request);
    }

    public Task Send(IRequest request)
    {
        request.NotNull();
        return _requestImplementation.SendRequest(request);
    }

    public Task Send(IMessage message)
    {
        message.NotNull();
        return _messageImplementation.SendMessage(message);
    }
}
