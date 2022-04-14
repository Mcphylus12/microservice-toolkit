using System.Linq.Expressions;

namespace Storage.Abstractions;

public interface IStore<T>
    where T : class, IEntity, new()
{
    Task<T> Add(T item);
    Task<T> Update(T item);
    Task<T?> Get(Guid id);
    Task<Response<T>> Get(Specification<T> specification);
    Task<Response<TProjection>> Get<TProjection>(Specification<T, TProjection> specification);
    Task Delete(Guid id);
    Task Delete(Expression<Func<T, bool>> predicate);
    Task Save();
}

public record class Response<T>(IEnumerable<T> Items, string ContinuationToken)
{
}
