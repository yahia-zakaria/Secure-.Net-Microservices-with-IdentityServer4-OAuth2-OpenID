using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistance;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        private readonly OrderContext context;
        public RepositoryBase(OrderContext context)
        {
            this.context = context;
        }
        public T Add(T entity)
        {
            context.Set<T>().Add(entity);
            return entity;
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync()
        {
            return await context.Set<T>().CountAsync() > 0;
        }

        public async Task<int> CountAsync()
        {
            return await context.Set<T>().CountAsync(); 
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = context.Set<T>();

            if (disableTracking)
                query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString))
                query = query.Include(includeString);

            if (!(predicate is null))
                query = query.Where(predicate);

            if (!(orderBy is null))
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = context.Set<T>();

            if (disableTracking)
                query = query.AsNoTracking();

            if (!(includes is null))
                query = includes.Aggregate(query,(current, include)=>current.Include(include));

            if (!(predicate is null))
                query = query.Where(predicate);

            if (!(orderBy is null))
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
           context.Set<T>().Remove(entity);
        }

        public void RemoveRangeAsync(List<T> entities)
        {
           context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
        }
    }
}
