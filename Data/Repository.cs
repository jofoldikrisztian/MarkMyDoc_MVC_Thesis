using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _entities = context.Set<T>();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate);
        }
        public void Add(T entity)
        {
            _entities.Add(entity);
        }
        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> predicate)
        {
            IQueryable<T> _entitiesWithEagerLoading = predicate(_entities);

            return await _entitiesWithEagerLoading.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
