using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MarkMyDoctor.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Remove(T entity);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> func);
    }
}
