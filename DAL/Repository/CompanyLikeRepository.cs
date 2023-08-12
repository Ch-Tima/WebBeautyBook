using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class CompanyLikeRepository : BaseRepository<CompanyLike, string>
    {
        public CompanyLikeRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
