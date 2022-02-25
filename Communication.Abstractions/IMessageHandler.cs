using Communication.Abstractions.Registration;

namespace Communication.Abstractions;

public interface IMessageHandler<in TMessage>
    where TMessage : IMessage
{
    Task HandleAsync(TMessage message, CancellationToken cancellationToken = default);
}
