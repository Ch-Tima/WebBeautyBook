using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class WorkerServiceRepository : BaseRepository<WorkerService, string>
    {
        public WorkerServiceRepository(BeautyBookDbContext db) : base(db)
        {
        }

    }
}
