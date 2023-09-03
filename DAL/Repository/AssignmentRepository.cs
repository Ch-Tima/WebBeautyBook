using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Assignment"/>.
    /// </summary>
    public class AssignmentRepository : BaseRepository<Assignment, string>
    {
        public AssignmentRepository(BeautyBookDbContext db) : base(db)
        {
        }

        public async Task<Assignment> GetAsync(string[] primaryKey)
        {
            return await _db.Assignments.FindAsync(primaryKey);
        }

        public async Task<Assignment?> FirstIncludeAsync(Expression<Func<Assignment, bool>> expression)
        {
            return await _db.Assignments
                .Include(x => x.Service)
                .Include(x => x.Worker)
                .Include(x => x.Appointments)
                .FirstOrDefaultAsync(expression);
        }

        public async Task DeleteAsync(Assignment assignment)
        {
            _db.Assignments.Remove(entity: assignment);
            await _db.SaveChangesAsync();
        }

    }
}
