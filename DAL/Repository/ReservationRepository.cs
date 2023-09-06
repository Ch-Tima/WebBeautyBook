using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Reservation"/>.
    /// </summary>
    public class ReservationRepository : BaseRepository<Reservation, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public ReservationRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
