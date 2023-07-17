using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class ClientRepository : BaseRepository<Client, string>
    {
        public ClientRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
