using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        private readonly UserManager<BaseUser> _userManager;
        private readonly ServiceService _serviceService;
        private readonly WorkerService _workerService;

        public ServiceController(UserManager<BaseUser> userManager, ServiceService serviceService,
            WorkerService workerService)
        {
            _userManager = userManager;
            _serviceService = serviceService;
            _workerService = workerService;
        }

        /// <summary>
        /// Get services for a specific company.
        /// </summary>
        /// <param name="compamyId">The ID of the company to fetch services for.</param>
        /// <returns>Returns a list of services for the company.</returns>
        [HttpGet("getServicesForCompany/{compamyId}")]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}, {Roles.WORKER}")]
        public async Task<IActionResult> GetServicesForCompany(string compamyId)
        {
            var user = await _userManager.GetUserAsync(User);
            var worker = await _workerService.GetAsync(user.WorkerId);
            if (worker == null || worker.CompanyId != compamyId)
                return BadRequest("Access is denied!");

            return Ok((await _serviceService.GetAllFindAsync(x => x.CompanyId == compamyId)));
        }

        /// <summary>
        /// Create a new service for a company.
        /// </summary>
        /// <param name="model">The service model containing service details.</param>
        /// <returns>Returns the created service if successful; otherwise, returns an error message.</returns>
        [HttpPut]
        [Authorize(Roles = Roles.OWN_COMPANY + "," + Roles.MANAGER)]
        public async Task<IActionResult> CreateService([FromBody] ServiceModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return BadRequest("User not found.");

            var worker = await _workerService.GetAsync(user.WorkerId);
            if (worker == null)
                return BadRequest("Most likely, you do not belong to any company.");

            var service = new Service()
            {
                Name = model.Name,
                Description = model.Description,
                Time = model.Time.ToTimeSpan(),
                Price = model.Price,
                CategoryId = model.CategoryId,
                CompanyId = worker.CompanyId
            };

            var result = await _serviceService.InsertAsync(service);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(service);
        }

        /// <summary>
        /// Update an existing service.
        /// </summary>
        /// <param name="model">The service model containing updated details.</param>
        /// <param name="Id">The ID of the service to update.</param>
        /// <returns>Returns success if the service is updated; otherwise, returns an error message.</returns>
        [HttpPost()]
        [Authorize(Roles = Roles.OWN_COMPANY + "," + Roles.MANAGER)]
        public async Task<IActionResult> UpdateService([FromBody] ServiceModel model, string Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var workerProfile = await _workerService.GetAsync(user.WorkerId);
            if (workerProfile == null)
                return BadRequest("Most likely, you do not belong to any company.");

            var result = await _serviceService.UpdataAsync(new Service()
            {
                Id = Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Time = model.Time.ToTimeSpan(),
                CategoryId = model.CategoryId,
                CompanyId = workerProfile.CompanyId
            });
            if (!result.IsSuccess) 
                return BadRequest(result.Message);

            return Ok();
        }

    }
}
