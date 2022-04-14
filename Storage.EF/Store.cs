using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Storage.Abstractions;

namespace Storage.EF;
public class Store<T> : IStore<T>
    where T : class, IEntity, new()
{
    private readonly DbSet<T> _set;
    private readonly DbContext _dbContext;

    public Store(DbContext dbContext)
    {
        _set = dbContext.Set<T>();
        _dbContext = dbContext;
    }

    public async Task<T> Add(T item)
    {
        var entry = await _set.AddAsync(item);
        return entry.Entity;
    }

    public Task Delete(Guid id)
    {
        var item = new T()
        {
            Id = id
        };
        _set.Attach(item);
        _set.Remove(item);
        return Task.CompletedTask;
    }

    public Task Delete(Expression<Func<T, bool>> predicate)
    {
        _set.RemoveRange(_set.Where(predicate));
        return Task.CompletedTask;
    }

    public async Task<T?> Get(Guid id)
    {
        return await _set.FindAsync(id);
    }

    public Task<Response<T>> Get(Specification<T> specification)
    {
        var query = specification.Query;

        var set = _set;
    }

    public Task<Response<TProjection>> Get<TProjection>(Specification<T, TProjection> specification)
    {
        throw new NotImplementedException();
    }

    public Task Save()
    {
        return _dbContext.SaveChangesAsync();
    }

    public Task<T> Update(T item)
    {
        var entry = _dbContext.Update(item);
        return Task.FromResult(entry.Entity);
    }
}
