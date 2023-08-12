using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                .Include(x => x.CompanyImages)
                .FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Company>> GetAllIncludeAsync()
        {
            return await _db.Companies
                .Include(x => x.Location)
                .Include(x => x.Workers)
                .Include(x => x.Services)
                .Include(x => x.CompanyOpenHours)
                .Include(x => x.CompanyImages)
                .ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetFindIncludeAsync(Expression<Func<Company, bool>> expression)
        {
            return await _db.Companies
                .Where(expression)
                .Include(x => x.Location)
                .Include(x => x.Workers)
                .Include(x => x.Services)
                .Include(x => x.CompanyOpenHours)
                .Include(x => x.CompanyImages)
                .ToListAsync();
        }

    }
}
