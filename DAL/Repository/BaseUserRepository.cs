using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="BaseUser"/> entities.
    /// </summary>
    public class BaseUserRepository : BaseRepository<BaseUser, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public BaseUserRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
