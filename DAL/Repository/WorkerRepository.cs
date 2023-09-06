using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Worker"/> prfile.
    /// </summary>
    public class WorkerRepository : BaseRepository<Worker, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public WorkerRepository(BeautyBookDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Retrieves a Worker entity by its primary key and includes related entities asynchronously.
        /// </summary>
        /// <param name="primaryKey">The primary key of the Worker to retrieve.</param>
        /// <returns>An asynchronous operation that returns the Worker entity with included related entities.</returns>
        public async Task<Worker?> GetIncudeAsync(string primaryKey)
        {
            return await _db.Workers
                .Include(x => x.Reservations)
                .Include(x => x.Appointments)
                .Include(x => x.BaseUser)
                .Include(x => x.Company)
                .FirstAsync(x => x.Id == primaryKey);
        }

        /// <summary>
        /// Retrieves a collection of Worker entities that match a specified expression and includes related entities asynchronously.
        /// </summary>
        /// <param name="expression">The expression to filter Worker entities.</param>
        /// <returns>An asynchronous operation that returns a collection of Worker entities with included related entities.</returns>
        public async Task<IEnumerable<Worker>> GetAllFindIncludeAsync(Expression<Func<Worker, bool>> expression)
        {
            return await _db.Workers.Where(expression)
                .Include(x => x.BaseUser)
                .Include(x => x.Company)
                .Include(x => x.Reservations)
                .Include(x => x.Appointments)
                .ToListAsync();
        }

    }
}
