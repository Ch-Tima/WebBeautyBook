using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class BaseUserRepository : BaseRepository<BaseUser, string>
    {
        public BaseUserRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
