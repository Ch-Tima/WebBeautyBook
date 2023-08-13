using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class CompanyLikeRepository : BaseRepository<CompanyLike, string>
    {
        public CompanyLikeRepository(BeautyBookDbContext db) : base(db)
        {
        }

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
