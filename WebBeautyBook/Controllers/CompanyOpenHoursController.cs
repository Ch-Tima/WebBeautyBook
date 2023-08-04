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
    public class CompanyOpenHoursController : ControllerBase
    {
        private readonly CompanyOpenHoursService _companyOpenHoursService;
        private readonly UserManager<BaseUser> _userManager;
        private readonly BLL.Services.WorkerService _workerService;

        public CompanyOpenHoursController(CompanyOpenHoursService companyOpenHoursService, 
            UserManager<BaseUser> userManager, BLL.Services.WorkerService workerService)
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
                var user = await _userManager.GetUserAsync(User);
                var workerProfile = await _workerService.GetAsync(user.WorkerId);

                if(workerProfile == null)
                    return BadRequest("Most likely, you do not belong to any company.");

                var openHours = new CompanyOpenHours() 
                {
                    CompanyId = workerProfile.CompanyId,
                    DayOfWeek = model.DayOfWeek,
                    OpenFrom = model.OpenFrom.ToTimeSpan(),
                    OpenUntil = model.OpenUntil.ToTimeSpan()
                };

                var result = await _companyOpenHoursService.AddAsync(openHours, workerProfile.CompanyId);
                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 

    }
}
