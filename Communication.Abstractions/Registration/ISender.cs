namespace Communication.Abstractions.Registration;

public interface ISender
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
    Task Send(IRequest request);
    Task Send(IMessage message);
}

public interface IRequest
{
}

public interface IRequest<TResponse> : IRequest
{
}

public interface IMessage
{
}
