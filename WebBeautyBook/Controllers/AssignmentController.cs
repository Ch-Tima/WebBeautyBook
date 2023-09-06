using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly UserManager<BaseUser> _userManager;
        private readonly AssignmentService _assignmentService;
        private readonly WorkerService _workerService;

        public AssignmentController(AssignmentService assignmentService, WorkerService workerService, UserManager<BaseUser> userManager)
        {
            _assignmentService = assignmentService;
            _workerService = workerService;
            _userManager = userManager;
        }

        /// <summary>
        /// Inserts a worker into a service assignment.
        /// </summary>
        /// <param name="model">The WorkerServiceModel containing assignment details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("insertWorkerToService")]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> InsertWorkerToService([FromBody] WorkerServiceModel model)
        {
            var workerProfile = await getWorkerProfile(User);
            if (workerProfile == null)//Check if the user belongs to any company, and return a BadRequest if not.
                return BadRequest("Most likely, you do not belong to any company.");
            //Create a new worker-service assignment with specified details.
            var workerService = new Assignment()
            {
                IsBlock = true,
                WorkerId = model.workerId,
                ServiceId = model.serviceId,
            };
            //Insert the assignment and return the result.
            var result = await _assignmentService.InsertAsync(workerProfile.CompanyId, workerService);
            if (!result.IsSuccess) 
                return BadRequest(result.Message);

            return Ok();
        }

        /// <summary>
        /// Removes a worker from a service assignment.
        /// </summary>
        /// <param name="model">The WorkerServiceModel containing assignment details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>

        [HttpPost("removeWorkerFromService")]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> RemoveWorkerFromService([FromBody] WorkerServiceModel model)
        {
            //Retrieve the worker's profile based on the authenticated user.
            var workerProfile = await getWorkerProfile(User);
            if (workerProfile == null)//Check if the user belongs to any company, and return a BadRequest if not.
                return BadRequest("Most likely, you do not belong to any company.");
            //Delete the assignment and return the result.
            var result = await _assignmentService.DeleteAsync(workerProfile.CompanyId, model.workerId, model.serviceId);
            if (!result.IsSuccess) 
                return BadRequest(result.Message);

            return Ok();
        }

        /// <summary>
        /// Retrieves the worker profile associated with the provided ClaimsPrincipal.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal representing the authenticated user.</param>
        /// <returns>The worker profile or null if not found.</returns>
        private async Task<Worker?> getWorkerProfile(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            var workerProfile = await _workerService.GetAsync(user.WorkerId);
            return workerProfile;
        }
    }
}
