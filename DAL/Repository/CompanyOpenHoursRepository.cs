using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="CompanyOpenHours"/> entities.
    /// </summary>
    public class CompanyOpenHoursRepository : BaseRepository<CompanyOpenHours, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyOpenHoursRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public CompanyOpenHoursRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
