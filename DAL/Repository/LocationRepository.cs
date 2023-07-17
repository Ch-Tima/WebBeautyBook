using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class LocationRepository : BaseRepository<Location, string>
    {
        public LocationRepository(BeautyBookDbContext db) : base(db)
        {

        }

        public async Task<IEnumerable<Location>> GetAllOnlyLocations() 
        {
           return (await _db.Locations.ToListAsync()).Select(x => new Location()
           {
               Id = x.Id,
               City = x.City,
               Country = x.Country,
           });
        }

    }
}
