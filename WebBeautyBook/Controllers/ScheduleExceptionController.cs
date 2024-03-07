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
    [Authorize]
    public class ScheduleExceptionController : ControllerBase
    {
        private readonly WorkerService _workerService;
        private readonly UserManager<BaseUser> _userManager;
        private readonly CompanyScheduleExceptionService _companyScheduleExceptionService;


        public ScheduleExceptionController(CompanyScheduleExceptionService companyScheduleExceptionService, WorkerService workerService, UserManager<BaseUser> userManager)
        {
            _companyScheduleExceptionService = companyScheduleExceptionService;
            _workerService = workerService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}, {Roles.WORKER}")]
        public async Task<IActionResult> Get()
        {
            var worker = (await HasWorkerProfile());
            // Check if the user has a worker profile.
            if (!(await HasWorkerProfile()).has)
                return BadRequest("Most likely, you do not belong to any company.");
            return Ok(await _companyScheduleExceptionService.FindAsync(worker.worker.CompanyId));
        }

        [HttpPut]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> Add([FromBody] CompanyScheduleExceptionViewModel model)
        {
            try
            {

                var worker = (await HasWorkerProfile());
                // Check if the user has a worker profile.
                if (!(await HasWorkerProfile()).has)
                    return BadRequest("Most likely, you do not belong to any company.");

                var data = new CompanyScheduleException()
                {
                    CompanyId = worker.worker.CompanyId,
                    ExceptionDate = model.ExceptionDate.LocalDateTime,
                    IsClosed = model.IsClosed,
                    IsOnce = model.IsOnce,
                    OpenFrom = model.OpenFrom.ToTimeSpan(),
                    OpenUntil = model.OpenUntil.ToTimeSpan(),
                    Reason = model.Reason,
                };

                var result = await _companyScheduleExceptionService.AddAsync(data);

                if (result.IsSuccess)
                    return Ok(data);

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> Remove([FromQuery] string id)
        {
            try
            {
                var worker = (await HasWorkerProfile());
                // Check if the user has a worker profile.
                if (!(await HasWorkerProfile()).has)
                    return BadRequest("Most likely, you do not belong to any company.");

                var result = await _companyScheduleExceptionService.DeleteAsynce(id, worker.worker.CompanyId);

                return Ok();
            }
            catch (Exception ex)
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
