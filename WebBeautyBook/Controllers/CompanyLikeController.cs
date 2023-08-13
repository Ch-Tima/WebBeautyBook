using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyLikeController : ControllerBase
    {
        private readonly CompanyLikeService _companyLikeService;
        private readonly UserManager<BaseUser> _userManager;

        public CompanyLikeController(CompanyLikeService companyLikeService, UserManager<BaseUser> userManager)
        {
            _companyLikeService = companyLikeService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<CompanyLike>> GetAllMine()
        {
            return await _companyLikeService.GetMineAsync(_userManager.GetUserId(User));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(string companyId)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _companyLikeService.AddAsync(new CompanyLike()
            {
                UserId = user.Id,
                CompanyId = companyId
            });

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string companyId)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _companyLikeService.DeleteAsync(companyId, user.Id);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();
        }

    }
}
