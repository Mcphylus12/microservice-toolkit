namespace Communication.Abstractions.Registration;

public interface IResolver
{
    Task<object?> Resolve(string messageType, Func<Type, Task<object>> deserialiseType);
}
