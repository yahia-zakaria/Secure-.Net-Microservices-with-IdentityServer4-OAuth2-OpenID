using Ordering.Domain.Common;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistance
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<List<T>> GetAllAsync(); 
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable, IOrderedQueryable> orderBy = null,
            string IncludeString = null, bool disableTracking = true);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable, IOrderedQueryable> orderBy = null,
    List<Expression<Func<T, object>>> Includes = null, bool disableTracking = true);
        Task<T> GetByIdAsync(int id);
        T Add(T entity);
        Task AddRangeAsync(List<T> entity);
        void Update(T entity);
        void Remove(T entity);
        Task RemoveRangeAsync(List<T> entity);
    }
}
