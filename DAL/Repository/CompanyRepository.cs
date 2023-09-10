using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Company"/> entities.
    /// </summary>
    public class CompanyRepository : BaseRepository<Company, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public CompanyRepository(BeautyBookDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Retrieves a Company entity by its ID and includes related entities asynchronously.
        /// </summary>
        /// <param name="id">The ID of the Company to retrieve.</param>
        /// <returns>An asynchronous operation that returns the Company entity with included related entities.</returns>
        public async Task<Company> GetIncludeAsync(string id)
        {
            return await _db.Companies
                .Include(x => x.Location)
                .Include(x => x.Workers)
                .Include(x => x.Services)
                .Include(x => x.CompanyOpenHours)
                .Include(x => x.CompanyImages)
                .FirstAsync(x => x.Id == id);
        }

        /// <summary>
        /// Retrieves all Company entities including related entities asynchronously.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of Company entities with included related entities.</returns>
        public async Task<IEnumerable<Company>> GetAllIncludeAsync()
        {
            return await _db.Companies
                .Include(x => x.Location)
                .Include(x => x.Workers)
                .Include(x => x.Services)
                .Include(x => x.CompanyOpenHours)
                .Include(x => x.CompanyImages)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a filtered collection of Company entities including related entities asynchronously.
        /// </summary>
        /// <param name="expression">The expression to filter Company entities.</param>
        /// <returns>An asynchronous operation that returns a collection of filtered Company entities with included related entities.</returns>
        public async Task<IEnumerable<Company>> GetFindIncludeAsync(Expression<Func<Company, bool>> expression)
        {
            return await _db.Companies
                .Where(expression)
                .Include(x => x.Location)
                .Include(x => x.Workers)
                .Include(x => x.Services)
                .Include(x => x.CompanyOpenHours)
                .Include(x => x.CompanyImages)
                .ToListAsync();
        }

    }
}
