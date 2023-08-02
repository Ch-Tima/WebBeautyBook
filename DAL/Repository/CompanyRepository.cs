using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class CompanyRepository : BaseRepository<Company, string>
    {
        public CompanyRepository(BeautyBookDbContext db) : base(db)
        {

        }


        public async Task<Company> GetIncludeAsync(string id)
        {
            return await _db.Companies
                .Include(x => x.Location)
                .Include(x => x.Workers)
                .Include(x => x.Services)
                .Include(x => x.CompanyOpenHours)
                .FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Company>> GetAllIncludeAsync()
        {
            return await _db.Companies
                .Include(x => x.Location)
                .Include(x => x.Workers)
                .Include(x => x.Services)
                .Include(x => x.CompanyOpenHours)
                .ToListAsync();
        }

        /*        public async Task<IEnumerable<Company>> GetAllIncludeAsync()
                {
                    return await _db.Companies.Include(x => x.Location).Include(x => x.Workers).Include(x => x.Services).ToListAsync();
                }*/

    }
}
