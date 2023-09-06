using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CategoryService
    {

        private readonly CategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The repository for categories.</param>
        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> GetAsync(string id)
        {
            return await _categoryRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetAllOnlyAsync()
        {
            return await _categoryRepository.GetAllOnlyAsync();
        }

        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            return await _categoryRepository.GetMainCategoriesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllFindAsync(Expression<Func<Category, bool>> func)
        {
            return await _categoryRepository.GetAllFindAsync(func);
        }

        /// <summary>
        /// Insert a new category into the repository.
        /// </summary>
        /// <param name="category">The category to insert.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<ServiceResponse> InsertAsync(Category category)
        {
            try
            {
                if (category.CategoryId == String.Empty) category.CategoryId = null;
                //Is there a subcategory
                if (category.CategoryId != null && !(await IsExist(category.CategoryId)))
                    return new ServiceResponse(false, "Not found sub category");

                if ((await _categoryRepository.GetFirstAsync(x => x.Name == category.Name)) != null)
                    return new ServiceResponse(false, $"Name {category.Name} is taken");

                await _categoryRepository.InsertAsync(category);
                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task DeleteAsync(string id)
        {
            //TODO: error Cascade
            await _categoryRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Update a category with new information.
        /// </summary>
        /// <param name="newCategory">The updated category information.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the update operation.</returns>
        public async Task<ServiceResponse> UpdataAsync(Category newCategory)
        {
            try
            {
                //Check if the newCategory parameter is null
                if (newCategory is null) return new ServiceResponse(false, "Parameter newCategory is null.");
                //Check if a category with the specified ID exists
                if (!await IsExist(newCategory.Id))
                    return new ServiceResponse(false, $"Category with id: {newCategory.Id} not found.");
                //Check if a category is trying to refer to itself (circular reference)
                if (newCategory.CategoryId == newCategory.Id)
                    return new ServiceResponse(false, "A category cannot refer to itself.");
                //Handle special case where CategoryId is an empty string and set it to null
                if (newCategory.CategoryId == "") newCategory.CategoryId = null;
                //Check if the referenced subcategory exists.
                if (newCategory.CategoryId != null && !(await IsExist(newCategory.CategoryId)))
                    return new ServiceResponse(false, "Not found sub category.");

                await _categoryRepository.UpdateAsync(newCategory.Id, newCategory);//Update
                var item = await _categoryRepository.GetAsync(newCategory.Id);//Retrieve the updated category from the repository
                //Check if the update was successful by comparing fields
                if (item.CategoryId == newCategory.CategoryId && item.Name == newCategory.Name) return new ServiceResponse(true, "Ok");
                else return new ServiceResponse(false, "Something went wrong during the update.");
            }
            catch(Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }

        }

        /// <summary>
        /// Check if a category with the specified ID exists.
        /// </summary>
        /// <param name="id">The ID of the category to check.</param>
        /// <returns>True if the category exists; otherwise, false.</returns>
        public async Task<bool> IsExist(string id)
        {
            try
            {
                if ((await GetAsync(id)) != null) return true;
                else return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}
