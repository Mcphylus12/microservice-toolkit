namespace Caching.Abstractions;

public interface ICache<TKey, TValue>
{
    Task<TValue> GetOrAdd(TKey key, Func<TValue> createItem);
}
