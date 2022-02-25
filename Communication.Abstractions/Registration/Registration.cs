using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Abstractions.Registration;

public static class Registration
{
    public static void RegisterCommunicationAbstractions(this IServiceCollection services)
    {
        var type = Assembly.GetEntryAssembly()?.GetType("Communication.Abstractions.Registration.HandlerRegistration");
        var method = type?.GetMethod("RegisterHandlers");
        method?.Invoke(null, new object?[] { services });
    }
}
