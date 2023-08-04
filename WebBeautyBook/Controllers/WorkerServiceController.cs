using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerServiceController : ControllerBase
    {
        private readonly UserManager<BaseUser> _userManager;
        private readonly WorkerServiceService _workerServiceService;
        private readonly BLL.Services.WorkerService _workerService;

        public WorkerServiceController(WorkerServiceService workerServiceService, BLL.Services.WorkerService workerService, UserManager<BaseUser> userManager)
        {
            _workerServiceService = workerServiceService;
            _workerService = workerService;
            _userManager = userManager;
        }

        [HttpPost("insertWorkerToService")]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> InsertWorkerToService([FromBody] WorkerServiceModel model)
        {
            var workerProfile = await getWorkerProfile(User);
            if (workerProfile == null)
                return BadRequest("Most likely, you do not belong to any company.");

            var workerService = new Domain.Models.WorkerService()
            {
                IsBlock = true,
                WorkerId = model.workerId,
                ServiceId = model.serviceId,
            };

            var result = await _workerServiceService.InsertAsync(workerProfile.CompanyId, workerService);

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok();
        }

        [HttpPost("removeWorkerFromService")]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> RemoveWorkerFromService([FromBody] WorkerServiceModel model)
        {
            var workerProfile = await getWorkerProfile(User);
            if (workerProfile == null)
                return BadRequest("Most likely, you do not belong to any company.");

            var result = await _workerServiceService.DeleteAsync(workerProfile.CompanyId, model.workerId, model.serviceId);

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok();
        }

        private async Task<Worker?> getWorkerProfile(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            var workerProfile = await _workerService.GetAsync(user.WorkerId);
            return workerProfile;
        }
    }
}
