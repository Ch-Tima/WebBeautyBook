using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Service"/> entities.
    /// </summary>
    public class ServiceRepository : BaseRepository<Service, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public ServiceRepository(BeautyBookDbContext db) : base(db)
        {
        }

    }
}
