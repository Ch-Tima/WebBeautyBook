using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    /// <summary>
    /// Controller for managing company open hours.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyOpenHoursController : ControllerBase
    {
        private readonly CompanyOpenHoursService _companyOpenHoursService;
        private readonly UserManager<BaseUser> _userManager;
        private readonly WorkerService _workerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyOpenHoursController"/> class.
        /// </summary>
        /// <param name="companyOpenHoursService">The service for company open hours.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="workerService">The service for workers.</param>
        public CompanyOpenHoursController(CompanyOpenHoursService companyOpenHoursService, 
            UserManager<BaseUser> userManager, WorkerService workerService)
        {
            _companyOpenHoursService = companyOpenHoursService;
            _userManager = userManager;
            _workerService = workerService;
        }

        /// <summary>
        /// Get company open hours by company ID.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<CompanyOpenHours>> Get(string companyId)
        {
            return await _companyOpenHoursService.FindAsync(companyId);
        }

        [HttpGet("GetWithException")]
        public async Task<IEnumerable<CompanyOpenHours>> GetWithException(string companyId)
        {
            return await _companyOpenHoursService.FindWithExeceptionAsync(companyId);
        }

        /// <summary>
        /// Add company open hours for a specific day.
        /// </summary>
        [HttpPut]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> Add([FromBody] OpenHoursModel model)
        {
            try
            {
                // Check if the user has a worker profile.
                var hasWorker = await HasWorkerProfile();

                if (!hasWorker.has)
                    return BadRequest("Most likely, you do not belong to any company.");

                // Create new company open hours object.
                var openHours = new CompanyOpenHours() 
                {
                    CompanyId = hasWorker.worker.CompanyId,
                    DayOfWeek = model.DayOfWeek,
                    OpenFrom = model.OpenFrom.ToTimeSpan(),
                    OpenUntil = model.OpenUntil.ToTimeSpan()
                };

                // Add new open hours
                var result = await _companyOpenHoursService.AddAsync(openHours);
                if (result.IsSuccess)
                    return Ok(openHours);

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update company open hours for a specific day.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> UpdateHours([FromBody] OpenHoursModel model, [FromQuery] string id)
        {
            try
            {
                // Check if the user has a worker profile.
                if (!(await HasWorkerProfile()).has)
                    return BadRequest("Most likely, you do not belong to any company.");

                var result = await _companyOpenHoursService.UpdateHoursAsync(id, model.OpenFrom.ToTimeSpan(), model.OpenUntil.ToTimeSpan());
                if(result.IsSuccess)
                    return Ok();

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete company open hours by ID.
        /// </summary>
        [HttpDelete]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> Remove([FromQuery]string id)
        {
            try
            {

                // Check if the user has a worker profile.
                if (!(await HasWorkerProfile()).has)
                    return BadRequest("Most likely, you do not belong to any company.");

                var result = await _companyOpenHoursService.DeleteAsynce(id);

                if(result.IsSuccess)
                    return Ok();
                
                return BadRequest(result.Message);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Checks if the current user has a worker profile.
        /// </summary>
        /// <returns>A tuple indicating whether the user has a worker profile and, if so, the worker profile information.</returns>
        private async Task<(bool has, Worker? worker)> HasWorkerProfile()
        {
            var user = await _userManager.GetUserAsync(User);// Retrieve the currently authenticated user
            var workerProfile = await _workerService.GetAsync(user.WorkerId);// Retrieve the worker profile associated with the user, if any
            return (workerProfile != null, workerProfile); // Return a tuple indicating whether the user has a worker profile and, if so, the worker profile information
        }

    }
}
