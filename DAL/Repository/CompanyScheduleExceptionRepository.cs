using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class CompanyScheduleExceptionRepository : BaseRepository<CompanyScheduleException, string>
    {
        public CompanyScheduleExceptionRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
