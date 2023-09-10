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
        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public AssignmentRepository(BeautyBookDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Retrieves the first <see cref="Assignment"/> entity that matches a specified expression, including related entities, asynchronously.
        /// </summary>
        /// <param name="expression">The expression to filter <see cref="Assignment"/> entities.</param>
        /// <returns>An asynchronous operation that returns the first matching <see cref="Assignment"/> entity with included related entities.</returns>
        public async Task<Assignment?> FirstIncludeAsync(Expression<Func<Assignment, bool>> expression) => await _db.Assignments
                .Include(x => x.Service)
                .Include(x => x.Worker)
                .Include(x => x.Appointments)
                .FirstOrDefaultAsync(expression);

        /// <summary>
        /// Deletes an <see cref="Assignment"/> entity asynchronously.
        /// </summary>
        /// <param name="assignment">The <see cref="Assignment"/> entity to delete.</param>

        public async Task DeleteAsync(Assignment assignment)
        {
            _db.Assignments.Remove(entity: assignment);
            await _db.SaveChangesAsync();
        }

    }
}
