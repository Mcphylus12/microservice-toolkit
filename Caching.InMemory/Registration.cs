using Caching.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Caching.InMemory;
public static class CachingRegistration
{
    public static void AddInMemoryCaching(this IServiceCollection services)
    {
        services.AddSingleton(typeof(ICache<,>), typeof(Cache<,>));
    }

    public static void AddInMemoryCaching<TKey, TValue>(this IServiceCollection services)
        where TKey : notnull
    {
        services.AddSingleton<ICache<TKey, TValue>, Cache<TKey, TValue>>();
    }
}
