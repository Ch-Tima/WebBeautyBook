using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Worker"/> prfile.
    /// </summary>
    public class WorkerRepository : BaseRepository<Worker, string>
    {

        public WorkerRepository(BeautyBookDbContext db) : base(db)
        {
        }

        public async Task<Worker?> GetIncudeAsync(string primaryKey)
        {
            return await _db.Workers
                .Include(x => x.Reservations)
                .Include(x => x.Assignments)
                .Include(x => x.BaseUser)
                .Include(x => x.Company)
                .FirstAsync(x => x.Id == primaryKey);
        }

        public async Task<IEnumerable<Worker>> GetAllFindIncludeAsync(Expression<Func<Worker, bool>> expression)
        {
            return await _db.Workers.Where(expression)
                .Include(x => x.BaseUser)
                .Include(x => x.Company)
                .Include(x => x.Reservations)
                .Include(x => x.Assignments)
                .ToListAsync();
        }

    }
}
