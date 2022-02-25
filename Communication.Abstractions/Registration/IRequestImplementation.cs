namespace Communication.Abstractions.Registration;

public interface IRequestImplementation
{
    Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request);
    Task SendRequest(IRequest request);
}
