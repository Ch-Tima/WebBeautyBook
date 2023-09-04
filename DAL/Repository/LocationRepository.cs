using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Location"/> entities.
    /// </summary>
    public class LocationRepository : BaseRepository<Location, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public LocationRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
