namespace Communication.Abstractions.Registration;

public interface IMessageImplementation
{
    Task SendMessage(IMessage message);
}
