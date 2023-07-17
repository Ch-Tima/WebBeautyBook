using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CategoryService
    {

        private readonly CategoryRepository _categoryRepository;

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

        public async Task InsertAsync(Category category)
        {
            if (category.CategoryId != null && category.CategoryId.Equals(""))
                category.CategoryId = null;

            await _categoryRepository.InsertAsync(category);
        }

        public async Task DeleteAsync(string id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<ServiceResponse> UpdataAsync(Category newCategory)
        {
            if (newCategory == null) return new ServiceResponse(false, "Parameter newCategory is null.");
            if (newCategory.CategoryId != null && newCategory.CategoryId.Length <= 0) newCategory.CategoryId = null;

            try
            {
                if (!await IsExist(newCategory.Id))
                    return new ServiceResponse(false, $"Category with id: {newCategory.Id} not found.");

                await _categoryRepository.UpdateAsync(newCategory.Id, newCategory);

                var item = await _categoryRepository.GetAsync(newCategory.Id);

                if (item.CategoryId == newCategory.CategoryId && item.Name == newCategory.Name) return new ServiceResponse(true, "Ok");
                else return new ServiceResponse(false, "Something went wrong during the update.");
            }
            catch(Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }

        }

        public async Task<bool> IsExist(string id)
        {
            try
            {
                if(await GetAsync(id) != null) return true;
                else return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}
