using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Elan.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        protected ElanDbContext _context;

        #region Properties

        public GenericRepository(ElanDbContext context)
        {
            _context = context;
        }

        #endregion Properties
        public virtual IQueryable<T> AsQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return _context.Set<T>().ToListAsync();
        }

        public virtual Task<int> CountAsync()
        {
            return _context.Set<T>().CountAsync();
        }

        public Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToListAsync();
        }

        public Task<T> GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefaultAsync();
        }

        public virtual Task<T> GetByAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual void Insert(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
