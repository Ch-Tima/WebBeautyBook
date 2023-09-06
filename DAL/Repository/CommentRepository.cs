using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Comment"/> entities.
    /// </summary
    public class CommentRepository : BaseRepository<Comment, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public CommentRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
