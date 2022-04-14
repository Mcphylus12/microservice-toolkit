using System.Linq.Expressions;

namespace Storage.Abstractions;

public class Specification<T>
{
    public Specification()
    {
        Query = new Query<T>();
    }

    public Query<T> Query { get; }
}

public class Query<T>
{
    public int? Limit { get; private set; }
    public string? ContinuationToken { get; private set; }
    public List<Expression<Func<T, bool>>> Predicates { get; }

    public List<(Expression<Func<T, object>> exp, Order order)> Orders { get; }

    internal Query()
    {
        Predicates = new List<Expression<Func<T, bool>>>();
        Orders = new List<(Expression<Func<T, object>>, Order)>();
    }

    public Query<T> Where(Expression<Func<T, bool>> predicate)
    {
        Predicates.Add(predicate);
        return this;
    }

    public Query<T> Order(Expression<Func<T, object>> propertyAccessor, Order order)
    {
        Orders.Add((propertyAccessor, order));
        return this;
    }

    public Query<T> Page(int limit, string continuationToken = "")
    {
        Limit = limit;
        ContinuationToken = continuationToken;
        return this;
    }
}

public enum Order
{
    Ascending,
    Descending
}

public class Specification<TOriginal, TProjection>
{
    public Specification()
    {
        Query = new SelectQuery<TOriginal, TProjection>();
    }

    public SelectQuery<TOriginal, TProjection> Query { get; }
}

public class SelectQuery<TOriginal, TProjection> : Query<TOriginal>
{
    public Expression<Func<TOriginal, TProjection>>? Projection { get; private set; }

    internal SelectQuery<TOriginal, TProjection> Select(Expression<Func<TOriginal, TProjection>> select)
    {
        Projection = select;
        return this;
    }
}
