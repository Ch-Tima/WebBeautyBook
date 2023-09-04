using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="Category"/> entities.
    /// </summary>
    public class CategoryRepository : BaseRepository<Category, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public CategoryRepository(BeautyBookDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Asynchronously returns all category objects with the selected properties.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of Category entities with selected properties.</returns>
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

        /// <summary>
        /// Retrieves the main Category entities (categories with no parent) asynchronously.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of main Category entities.</returns>
        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            var result = await _db.Categories.Where(x => x.CategoryId == null).ToListAsync();
            foreach (var item in result)
                item.Categories = ((List<Category>)(await UploadPropertyNavigation(item)));
            return result;
        }

        /// <summary>
        /// Recursively loads navigation properties for a given Category entity and its child categories.
        /// </summary>
        /// <param name="category">The Category entity for which to load navigation properties.</param>
        /// <returns>An asynchronous operation that returns a collection of Category entities with loaded navigation properties.</returns>
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
