using DAL.Context;
using Domain.Models;

namespace DAL.Repository
{
    public class CompanyImagesRepository : BaseRepository<CompanyImage, string>
    {
        public CompanyImagesRepository(BeautyBookDbContext db) : base(db)
        {
        }
    }
}
