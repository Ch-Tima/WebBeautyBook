using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="CompanyImage"/> entities.
    /// </summary>
    public class CompanyImagesRepository : BaseRepository<CompanyImage, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyImagesRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public CompanyImagesRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
