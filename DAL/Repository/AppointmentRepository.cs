using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Appointment"/>.
    /// </summary>
    public class AppointmentRepository : BaseRepository<Appointment, string>
    {
        public AppointmentRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
