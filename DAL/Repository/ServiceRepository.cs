using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class ServiceRepository : BaseRepository<Service, string>
    {
        public ServiceRepository(BeautyBookDbContext db) : base(db)
        {
        }

    }
}
