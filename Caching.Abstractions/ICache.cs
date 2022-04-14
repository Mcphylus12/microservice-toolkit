namespace Caching.Abstractions;

public interface ICache<TKey, TValue>
    where TKey : notnull
{
    Task<TValue> GetOrAdd(TKey key, Func<TKey, TValue> createItem);

    Task Set(TKey key, TValue value);

    Task Clear();
}
