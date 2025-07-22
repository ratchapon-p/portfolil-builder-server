using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Data;

public class GenericListRepository<T>(StoreContext context) : IGenericListRepository<T> where T : BaseEntity
{
    private IQueryable<T> FilterDeleted()
    {
        return context.Set<T>().Where(x => EF.Property<byte>(x, "IsDelete") != 1);
    }
    private static string GetPropertyName(Expression<Func<T, string>> propertyLambda)
    {
        if (propertyLambda.Body is MemberExpression member)
        {
            return member.Member.Name;
        }
        throw new ArgumentException("Expression must be property", nameof(propertyLambda));
    }
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public async Task<int> CountAsync(ISpecification<T> spec, int userId)
    {
        var query = FilterDeleted().Where(x => x.UserId == userId);
        // query = spec.ApplyPaging(query);

        return await query.CountAsync();
    }

    public bool Exists(Expression<Func<T, string>> propName, string uniqueName, int userId)
    {
        var propertyName = GetPropertyName(propName);

        return FilterDeleted().Any(x => x.UserId == userId && EF.Property<string>(x, propertyName) == uniqueName);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await FilterDeleted().FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
    }

    public async Task<IReadOnlyList<T>> GetListAsyncByUserId(int userId)
    {
        return await context.Set<T>().Where(x => x.UserId == userId).ToListAsync();
    }

    public void Remove(T entity)
    {
        context.Entry(entity).Property("IsDelete").CurrentValue = (byte)1;
        context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, int userId)
    {
        var query = FilterDeleted().Where(x => x.UserId == userId);

        query = spec.ApplyPaging(query);

        return await query.ToListAsync();
    }

}
