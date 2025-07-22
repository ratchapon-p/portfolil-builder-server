using System;
using System.Linq.Expressions;

namespace portfolio_builder_server.Interfaces;

public interface ISpecification<T>
{
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
    IQueryable<T> ApplyPaging(IQueryable<T> query);
}

