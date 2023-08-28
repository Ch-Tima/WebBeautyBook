using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Reservation"/>.
    /// </summary>
    public class ReservationRepository : BaseRepository<Reservation, string>
    {
        public ReservationRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
