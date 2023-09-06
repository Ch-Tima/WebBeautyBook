using DAL.Context;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    /// <summary>
    /// BaseRepository is an abstract class that implements the IGenericRepository interface.
    /// It provides common CRUD (Create, Read, Update, Delete) operations for data access.
    /// </summary>
    /// <typeparam name="T">The type of entity this repository handles</typeparam>
    /// <typeparam name="TPrimaryKey">The type of the primary key for the entity</typeparam>
    public abstract class BaseRepository<T, TPrimaryKey> : IGenericRepository<T, TPrimaryKey> where T : class
    {
        protected readonly BeautyBookDbContext _db;
        private readonly DbSet<T> entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T, TPrimaryKey}"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public BaseRepository(BeautyBookDbContext db) 
        {
            _db = db;
            entities = _db.Set<T>();// DbSet to work with entities of type T
        }

        /// <summary>
        /// Deletes an entity asynchronously based on its primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key of the entity to delete.</param>
        public virtual async Task DeleteAsync(TPrimaryKey primaryKey)
        {
            var obj = await entities.FindAsync(primaryKey);

            if (obj == null) return;
            else entities.Remove(entity: obj);

            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> asynchronously.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of all entities of type <typeparamref name="T"/>.</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }

        /// <summary>
        /// Retrieves entities of type <typeparamref name="T"/> that match a specified expression asynchronously.
        /// </summary>
        /// <param name="expression">The expression to filter entities.</param>
        /// <returns>An asynchronous operation that returns a collection of filtered entities.</returns>
        public virtual async Task<IEnumerable<T>> GetAllFindAsync(Expression<Func<T, bool>> expression)
        {
            return await entities.Where(expression).ToListAsync();
        }

        /// <summary>
        /// Retrieves an entity of type <typeparamref name="T"/> by its primary key asynchronously.
        /// </summary>
        /// <param name="primaryKey">The primary key of the entity to retrieve.</param>
        /// <returns>An asynchronous operation that returns the entity with the specified primary key.</returns>
        public virtual async Task<T> GetAsync(TPrimaryKey primaryKey)
        {
            return await entities.FindAsync(primaryKey);
        }

        /// <summary>
        /// Retrieves the first entity of type <typeparamref name="T"/> that matches a specified expression asynchronously.
        /// </summary>
        /// <param name="expression">The expression to filter entities.</param>
        /// <returns>An asynchronous operation that returns the first matching entity.</returns>
        public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> expression)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(expression);
        }

        /// <summary>
        /// Inserts a new entity of type <typeparamref name="T"/> asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public virtual async Task InsertAsync(T entity)
        {
            await entities.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing entity of type <typeparamref name="T"/> asynchronously based on its primary key.
        /// </summary>
        /// <param name="key">The primary key of the entity to update.</param>
        /// <param name="entity">The updated entity.</param>
        public virtual async Task UpdateAsync(TPrimaryKey key, T entity)
        {
            var local = await GetAsync(key);
 
            if (local != null)//detached
                _db.Entry(local).State = EntityState.Detached;

            // set Modified flag in your entry
            _db.Entry(entity).State = EntityState.Modified;

            await _db.SaveChangesAsync();//save changes to the database.
        }
    }
}
