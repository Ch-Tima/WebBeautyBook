using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly UserManager<BaseUser> _userManager;
        private readonly BLL.Services.WorkerService _workerService;

        public UserController(UserManager<BaseUser> userManager, BLL.Services.WorkerService workerService)
        {
            _userManager = userManager;
            _workerService = workerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return new BadRequestObjectResult("User not found!");

            return new OkObjectResult(new
            {
                name = user.UserName,
                surname = user.UserSurname,
                email = user.Email,
                photo = user.Photo,
                roles = await _userManager.GetRolesAsync(user),
                workerId = user.WorkerId,
                companyId = user.WorkerId != null ? (await _workerService.GetAsync(user.WorkerId))?.CompanyId : null
            });
        }

    }
}
