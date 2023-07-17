using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class CommentRepository : BaseRepository<Comment, string>
    {
        public CommentRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
