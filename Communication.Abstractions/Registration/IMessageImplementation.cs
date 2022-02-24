namespace Communication.Abstractions.Registration;

public interface IMessageImplementation
{
    Task SendMessage(string endpoint, IMessage message);
}
