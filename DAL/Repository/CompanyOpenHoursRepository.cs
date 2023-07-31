using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class CompanyOpenHoursRepository : BaseRepository<CompanyOpenHours, string>
    {
        public CompanyOpenHoursRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
