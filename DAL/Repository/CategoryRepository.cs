using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class CategoryRepository : BaseRepository<Category, string>
    {
        public CategoryRepository(BeautyBookDbContext db) : base(db)
        {

        }

        public async Task<IEnumerable<Category>> GetAllOnlyAsync()
        {
            return await _db.Categories.Select(x => new Category()
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                Categors = null
            }).ToListAsync();

        }

        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            var result = await _db.Categories
                .Where(x => x.CategoryId == null)
                .ToListAsync();

            foreach (var item in result)
                item.Categories = ((List<Category>)(await UploadPropertyNavigation(item)));

            return result;
        }

        private async Task<IEnumerable<Category>> UploadPropertyNavigation(Category category)
        {
            var result = new List<Category>();

            var listCategory = await _db.Categories
                .Where(x => x.CategoryId == category.Id)
                .Include(x => x.Categories).ToListAsync();

            foreach (var item in listCategory)
            {
                item.Categors = null;
                item.Services = null;

                if (item.Categories?.Count() > 0)
                    item.Categories.ToList().AddRange(await UploadPropertyNavigation(item));

                result.Add(item);
            }

            return result;
        }

    }
}
