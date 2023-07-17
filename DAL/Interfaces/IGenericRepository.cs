using System.Linq.Expressions;

namespace DAL.Interfaces
{
    public interface IGenericRepository<T, in TPrimaryKey> where T : class
    {

        public Task<T> GetAsync(TPrimaryKey primaryKey);
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> expression);

        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetAllFindAsync(Expression<Func<T, bool>> expression);

        public Task InsertAsync(T entity);

        public Task DeleteAsync(TPrimaryKey primaryKey);

        public Task UpdateAsync(TPrimaryKey key, T entity);
    }
}
