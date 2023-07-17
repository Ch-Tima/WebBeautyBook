using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet()]
        [AllowAnonymous]
        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryService.GetAllOnlyAsync(); ;
        }

        [HttpGet("GetMainCategories")]
        [AllowAnonymous]
        public async Task<IEnumerable<Category>> GetMainCategoriesAsync()
        {
            return await _categoryService.GetMainCategoriesAsync();
        }

        [HttpPut]
        public async Task<IActionResult> Insert([FromBody] CategoryModel model)
        {
            try
            {
                var category = new Category()
                {
                    Name = model.name,
                    CategoryId = model.categoryId,
                };

                var result = await _categoryService.InsertAsync(category);

                if (!result.IsSuccess) return BadRequest(result.Message);

                return new OkObjectResult(new
                {
                    id = category.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category newCategory)
        {
            var result = await _categoryService.UpdataAsync(newCategory);

            if (result.IsSuccess)
                return Ok();
            else
                return BadRequest(result.Message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                    return BadRequest($"This id {id} is not correct!");

                await _categoryService.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
