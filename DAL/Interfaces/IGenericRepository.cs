using System.Linq.Expressions;

namespace DAL.Interfaces
{

    /// <summary>
    /// Generic repository interface for working with entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type.</typepa
    public interface IGenericRepository<T, in TPrimaryKey> where T : class
    {
        /// <summary>
        /// Retrieves an entity by its primary key asynchronously.
        /// </summary>
        /// <param name="primaryKey">The primary key of the entity to retrieve.</param>
        /// <returns>An asynchronous operation that returns the entity with the specified primary key.</returns>
        public Task<T> GetAsync(TPrimaryKey primaryKey);

        /// <summary>
        /// Retrieves the first entity that matches a specified expression asynchronously.
        /// </summary>
        /// <param name="expression">The expression to filter entities.</param>
        /// <returns>An asynchronous operation that returns the first matching entity.</returns>
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> asynchronously.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of all entities.</returns>
        public Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves a collection of entities that match a specified expression asynchronously.
        /// </summary>
        /// <param name="expression">The expression to filter entities.</param>
        /// <returns>An asynchronous operation that returns a collection of filtered entities.</returns>
        public Task<IEnumerable<T>> GetAllFindAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public Task InsertAsync(T entity);

        /// <summary>
        /// Deletes an entity by its primary key asynchronously.
        /// </summary>
        /// <param name="primaryKey">The primary key of the entity to delete.</param>
        public Task DeleteAsync(TPrimaryKey primaryKey);

        /// <summary>
        /// Updates an entity asynchronously.
        /// </summary>
        /// <param name="key">The primary key of the entity to update.</param>
        /// <param name="entity">The entity with updated data.</param>
        public Task UpdateAsync(TPrimaryKey key, T entity);
    }
}
