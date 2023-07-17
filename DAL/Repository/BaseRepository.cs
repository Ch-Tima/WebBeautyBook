using DAL.Context;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public abstract class BaseRepository<T, TPrimaryKey> : IGenericRepository<T, TPrimaryKey> where T : class
    {
        protected readonly BeautyBookDbContext _db;
        private readonly DbSet<T> entities;

        public BaseRepository(BeautyBookDbContext db) 
        {
            _db = db;
            entities = _db.Set<T>();
        }


        public virtual async Task DeleteAsync(TPrimaryKey primaryKey)
        {
            var obj = await entities.FindAsync(primaryKey);

            if (obj == null) return;
            else entities.Remove(entity: obj);

            await _db.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllFindAsync(Expression<Func<T, bool>> expression)
        {
            return await entities.Where(expression).ToListAsync();
        }

        public virtual async Task<T> GetAsync(TPrimaryKey primaryKey)
        {
            return await entities.FindAsync(primaryKey);
        }

        public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> expression)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(expression);
        }

        public virtual async Task InsertAsync(T entity)
        {
            await entities.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TPrimaryKey key, T entity)
        {
            var local = await GetAsync(key);
 
            if (local != null)//detached
                _db.Entry(local).State = EntityState.Detached;

            // set Modified flag in your entry
            _db.Entry(entity).State = EntityState.Modified;

            //save
            await _db.SaveChangesAsync();
        }
    }
}
