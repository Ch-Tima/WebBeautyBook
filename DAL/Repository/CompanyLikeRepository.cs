using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="CompanyLike"/> entities.
    /// </summary>
    public class CompanyLikeRepository : BaseRepository<CompanyLike, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyLikeRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public CompanyLikeRepository(BeautyBookDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Retrieves a filtered collection of <see cref="CompanyLike"/> entities including related entities asynchronously.
        /// </summary>
        /// <param name="expression">The expression to filter <see cref="CompanyLike"/> entities.</param>
        /// <returns>An asynchronous operation that returns a collection of filtered <see cref="CompanyLike"/> entities with included related entities.</returns>
        public async Task<IEnumerable<CompanyLike>> GetAllFindIncludeAsync(Expression<Func<CompanyLike, bool>> expression)
        {
            return await _db.CompanyLikes
                .Include(x => x.User)
                .Include(x => x.Company)
                .Include(x => x.Company.Location)
                .Include(x => x.Company.CompanyImages)
                .Include(x => x.Company.CompanyOpenHours)
                .Where(expression).ToListAsync();
        }

    }
}
