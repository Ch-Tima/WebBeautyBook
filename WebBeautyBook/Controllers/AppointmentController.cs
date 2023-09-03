using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly UserManager<BaseUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AppointmentController(AppointmentService appointmentService, AssignmentService assignmentService, UserManager<BaseUser> userManager, IEmailSender emailSender)
        {
            _appointmentService = appointmentService;
            _assignmentService = assignmentService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet(nameof(GetMyAppointments))]
        [Authorize]
        public async Task GetMyAppointments()
        {

        }

        [HttpGet(nameof(GetCompanyAppointments))]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.WORKER}")]
        public async Task GetCompanyAppointments()
        {

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
