namespace Communication.Abstractions.Registration;

public interface IRequestImplementation
{
    Task<TResponse> SendRequest<TResponse>(string endpoint, IRequest<TResponse> request);
    Task SendRequest(string endpoint, IRequest request);
}
