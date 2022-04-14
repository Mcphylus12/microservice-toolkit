using System.Collections.Concurrent;
using Caching.Abstractions;

namespace Caching.InMemory;
public class Cache<TKey, TValue> : ICache<TKey, TValue>
    where TKey : notnull
{
    private ConcurrentDictionary<TKey, TValue> _data;

    public Cache()
    {
        _data = new ConcurrentDictionary<TKey, TValue>();
    }

    public Task Clear()
    {
        _data.Clear();
        return Task.CompletedTask;
    }

    public Task<TValue> GetOrAdd(TKey key, Func<TKey, TValue> createItem)
    {
        return Task.FromResult(_data.GetOrAdd(key, createItem));
    }

    public Task Set(TKey key, TValue value)
    {
        _data[key] = value;
        return Task.CompletedTask;
    }
}
