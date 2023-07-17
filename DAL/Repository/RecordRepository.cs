using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class RecordRepository : BaseRepository<Record, string>
    {
        public RecordRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
