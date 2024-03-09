using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class CompanyScheduleExceptionRepository : BaseRepository<CompanyScheduleException, string>
    {
        public CompanyScheduleExceptionRepository(BeautyBookDbContext db) : base(db)
        {
        }

        public async Task<bool> AnyAsync(Expression<Func<CompanyScheduleException, bool>> expression)
        {
            return await _db.CompanyScheduleExceptions.AnyAsync(expression);
        }

    }
}
