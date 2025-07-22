
using System.Linq.Expressions;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Data
{
public class PaginationSpecification <T> : ISpecification<T>
    {
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPagingEnabled { get; private set; }
        private Expression<Func<T, object>>? _orderBy;

        public PaginationSpecification()
        {
            IsPagingEnabled = false;
            Skip = 0;
            Take = 0;
            
            var idProp = typeof(T).GetProperty("Id");
            if (idProp != null)
            {
                var param = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(param, "Id");
                var converted = Expression.Convert(property, typeof(object));
                _orderBy = Expression.Lambda<Func<T, object>>(converted, param);
            }
        }

        public void ApplyOrderBy(Expression<Func<T, object>> orderBy)
        {
            _orderBy = orderBy;
        }

        public void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        public IQueryable<T> ApplyPaging(IQueryable<T> query)
        {
            if (_orderBy != null)
            {
                query = query.OrderBy(_orderBy);
            }
            if (IsPagingEnabled)
            {
                query = query.Skip(Skip).Take(Take);
            }
            return query;
        }
    }
}