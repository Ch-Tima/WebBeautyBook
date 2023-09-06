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
    public class AppointmentController : ControllerBase
    {

        private readonly AppointmentService _appointmentService;
        private readonly AssignmentService _assignmentService;
        private readonly WorkerService _workerService;
        private readonly UserManager<BaseUser> _userManager;
        public AppointmentController(AppointmentService appointmentService, AssignmentService assignmentService, 
            WorkerService workerService, UserManager<BaseUser> userManager)
        {
            _appointmentService = appointmentService;
            _assignmentService = assignmentService;
            _workerService = workerService;
            _userManager = userManager;
        }


        [Authorize()]
        [HttpGet(nameof(GetMyAppointments))]
        public async Task<IEnumerable<Appointment>> GetMyAppointments()
        {
            var user = await _userManager.GetUserAsync(User);
            return await _appointmentService.GetAllFindIncludeAsync(x => x.UserId == user.Id);
        }

        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.WORKER}")]
        [HttpGet(nameof(GetAppointmentsForMyCompany))]
        public async Task<IEnumerable<Appointment>> GetAppointmentsForMyCompany([FromQuery] string[] ids)
        {
            var user = await _userManager.GetUserAsync(User);
            var worker = await _workerService.GetAsync(user.WorkerId);

            if (worker == null)
                return new List<Appointment>();

            return await _appointmentService.GetAllFindAsync(r => r.Worker.CompanyId == worker.CompanyId && ids.Contains(r.WorkerId));
        }

        [Authorize]
        [HttpPut(nameof(CreateAppointmentViaClient))]
        public async Task<IActionResult> CreateAppointmentViaClient([FromBody] AppointmentViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.WorkerId == model.WorkerId) return BadRequest("You can not do that.");

            var appointment = new Appointment()
            {
                Date = model.Date.LocalDateTime,
                Note = model.Note,
                Status = AppointmentStatus.done,
                TimeStart = model.Time.ToTimeSpan(),
                UserId = user.Id,
                WorkerId = model.WorkerId,
                ServiceId = model.ServiceId,
            };

            var result = await _appointmentService.InsertAsync(appointment);
            if(!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();
        }

    }
}
