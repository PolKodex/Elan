using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Elan.Data
{
    public interface IGenericRepository<T> where T : class, new()
    {
        IQueryable<T> AsQueryable();
        Task<int> CountAsync();
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Insert(T entity);
        void Update(T entity);
        Task<int> SaveChangesAsync();
    }
}