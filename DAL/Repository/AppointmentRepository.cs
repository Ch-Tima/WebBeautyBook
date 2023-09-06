using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Appointment"/>.
    /// </summary>
    public class AppointmentRepository : BaseRepository<Appointment, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public AppointmentRepository(BeautyBookDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Retrieves a collection of appointments asynchronously based on the specified filter expression, including related entities.
        /// </summary>
        /// <param name="func">The filtering expression to apply to the appointments.</param>
        /// <returns>
        /// An asynchronous task that represents the operation. The task result contains a collection of appointments that match the specified filter and include related entities.
        /// </returns>
        public async Task<IEnumerable<Appointment>> GetAllFindIncludeAsync(Expression<Func<Appointment, bool>> func)
        {
            return await _db.Appointments.Where(func)
                .Include(x => x.Service)
                .Include(x => x.Worker)
                .Include(x => x.Comment)
                .Include(x => x.BaseUser)
                .ToListAsync();
        }

    }
}
