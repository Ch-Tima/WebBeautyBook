using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class ScheduleRepository : BaseRepository<Schedule, string>
    {
        public ScheduleRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
