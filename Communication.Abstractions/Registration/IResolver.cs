namespace Communication.Abstractions.Registration;

public interface IResolver
{
    public delegate Task<object> Deserialise(Type type);
    Task<object?> Resolve(string messageType, Deserialise deserialiseType);
}
