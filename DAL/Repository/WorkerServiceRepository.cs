using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class WorkerServiceRepository : BaseRepository<WorkerService, string>
    {
        public WorkerServiceRepository(BeautyBookDbContext db) : base(db)
        {
        }

        public async Task<WorkerService?> GetFirstIncludeAsync(Expression<Func<WorkerService, bool>> expression)
        {
            return await _db.WorkersService
                .Include(x => x.Service)
                .Include(x => x.Worker)
                .Include(x => x.Records)
                .FirstOrDefaultAsync(expression);
        }

    }
}
