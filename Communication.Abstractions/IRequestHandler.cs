using Communication.Abstractions.Registration;

namespace Communication.Abstractions;

public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
