namespace Storage.Abstractions;

internal interface IStore<T>
{
    Task<T> Add(T item);
    Task<T> Update(T item);
    Task<T> Get(Guid id);
    Task<IEnumerable<T>> Get(ISpecification<T> specification);
    Task<IEnumerable<TProjection>> Get<TProjection>(ISpecification<T, TProjection> specification);
    Task Delete(Guid id);
    Task Delete(ISpecification<T> specification);
    Task Save();
}
