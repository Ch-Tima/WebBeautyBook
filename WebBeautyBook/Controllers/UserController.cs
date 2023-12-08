using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBeautyBook.Helpers;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly UserManager<BaseUser> _userManager;
        private readonly WorkerService _workerService;
        private readonly BaseUserService _baseUserService;
        private readonly IWebHostEnvironment _appEnvironment;

        public UserController(UserManager<BaseUser> userManager, WorkerService workerService, BaseUserService baseUserService, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _workerService = workerService;
            _baseUserService = baseUserService;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return new BadRequestObjectResult("User not found!");
            return new OkObjectResult(await DeleteSensitiveData(user));
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm]UserUpdateModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.Name;
                user.UserSurname = model.Surname;

                if (model.File != null)
                {
                    //save new user icon
                    var name = $"{Guid.NewGuid()}.webp";
                    var resultSaveImage = await model.File.SaveToWebpAsync($"{_appEnvironment.WebRootPath}/images/userIcons/{name}");

                    if (!resultSaveImage.IsSuccess)
                        return StatusCode(500, resultSaveImage.Message);

                    if (user.Photo.IndexOf("userIcons") != -1)//remove the old icon if it's not the default icon
                        FilesHelper.Delete($"{_appEnvironment.WebRootPath}/{user.Photo}");

                    user.Photo = $"/images/userIcons/{name}";//set new path to image
                }

                var resultUpdate = await _baseUserService.UpdateAsync(user);

                if(!resultUpdate.IsSuccess)
                    return BadRequest(resultUpdate.Message);

                return new OkObjectResult(await DeleteSensitiveData(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }   
        }


        /// <summary>
        /// Generates a new model without sensitive fields based on the <see cref="BaseUser"/>.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<UserResponseModel> DeleteSensitiveData(BaseUser user)
        {
            return new UserResponseModel()
            {
                Name = user.UserName,
                Surname = user.UserSurname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Photo = user.Photo,
                Roles = await _userManager.GetRolesAsync(user),
                WorkerId = user.WorkerId,
                CompanyId = user.WorkerId != null ? (await _workerService.GetAsync(user.WorkerId))?.CompanyId : null
            };
        }

    }
}
