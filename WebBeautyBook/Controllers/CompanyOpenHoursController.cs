using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Tga;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyOpenHoursController : ControllerBase
    {
        private readonly CompanyOpenHoursService _companyOpenHoursService;
        private readonly UserManager<BaseUser> _userManager;
        private readonly WorkerService _workerService;

        public CompanyOpenHoursController(CompanyOpenHoursService companyOpenHoursService, 
            UserManager<BaseUser> userManager, WorkerService workerService)
        {
            _companyOpenHoursService = companyOpenHoursService;
            _userManager = userManager;
            _workerService = workerService;
        }

        [HttpGet]
        public async Task<IEnumerable<CompanyOpenHours>> Get(string companyId)
        {
            return await _companyOpenHoursService.FindAsync(companyId);
        }

        [HttpPut]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> Add([FromBody] OpenHoursModel model)
        {
            try
            {
                var hasWorker = await HasWorkerProfile();

                if (!hasWorker.has)
                    return BadRequest("Most likely, you do not belong to any company.");

                var openHours = new CompanyOpenHours() 
                {
                    CompanyId = hasWorker.worker.CompanyId,
                    DayOfWeek = model.DayOfWeek,
                    OpenFrom = model.OpenFrom.ToTimeSpan(),
                    OpenUntil = model.OpenUntil.ToTimeSpan()
                };

                var result = await _companyOpenHoursService.AddAsync(openHours, hasWorker.worker.CompanyId);
                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> UpdateHours([FromBody] OpenHoursModel model, [FromQuery] string id)
        {
            try
            {
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

        [HttpDelete]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}")]
        public async Task<IActionResult> Remove([FromQuery]string id)
        {
            try
            {

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

        private async Task<(bool has, Worker? worker)> HasWorkerProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var workerProfile = await _workerService.GetAsync(user.WorkerId);

             return (workerProfile != null, workerProfile);
        }

    }
}
