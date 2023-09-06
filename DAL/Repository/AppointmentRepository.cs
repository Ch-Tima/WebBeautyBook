using DAL.Context;
using Domain.Models;

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
    }
}
