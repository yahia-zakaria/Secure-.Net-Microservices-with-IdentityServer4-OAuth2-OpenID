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
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null, bool disableTracking = true);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    List<Expression<Func<T, object>>> includes = null, bool disableTracking = true);
        Task<T> GetByIdAsync(int id);
        Task<int> CountAsync();
        Task<bool> AnyAsync();
        T Add(T entity);
        Task AddRangeAsync(List<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRangeAsync(List<T> entities);
    }
}
