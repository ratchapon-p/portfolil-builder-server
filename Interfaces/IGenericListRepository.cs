using System;
using System.Linq.Expressions;
using portfolio_builder_server.Entities;

namespace portfolio_builder_server.Interfaces;

public interface IGenericListRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetListAsyncByUserId(int id); //* Get list async by user id
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(Expression<Func<T, string>> propName, string uniqueName, int userId);
    Task<int> CountAsync(ISpecification<T> spec,int userId);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec,int userId);

}
