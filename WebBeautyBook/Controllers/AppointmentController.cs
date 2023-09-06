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
        private readonly WorkerService _workerService;
        private readonly UserManager<BaseUser> _userManager;
        private readonly BaseUserService _baseUserService;
        private readonly ServiceService _serviceService;
        private readonly IEmailSender _emailService;

        public AppointmentController(AppointmentService appointmentService, AssignmentService assignmentService, 
            WorkerService workerService, UserManager<BaseUser> userManager, BaseUserService baseUserService, 
            ServiceService serviceService, IEmailSender emailService)
        {
            _appointmentService = appointmentService;
            _assignmentService = assignmentService;
            _workerService = workerService;
            _userManager = userManager;
            _baseUserService = baseUserService;
            _serviceService = serviceService;
            _emailService = emailService;
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

            var worker = await _baseUserService.FirstAsync(x => x.WorkerId == model.WorkerId);
            var service = await _serviceService.GetAsync(appointment.ServiceId);
            if(worker != null)
            {
                await _emailService.SendEmailAsync(worker.Email, "You have a new entry!", $"<label>User \"{user.UserName} {user.UserSurname}\" signed up for your \"{service.Name}\" service.</label>" +
                    $"<p>Details: {appointment.Date.ToLocalTime().ToString("dd-MM-yyyy")} {appointment.TimeStart.ToString(@"hh\:mm")} for \"{service.Name}\" service, price {service.Price}$.</p>" +
                    $"<p>Note from client:{appointment.Note}</p>" +
                    $"<p>&emsp;Please confirm or reject the client's booking in your personal account.</p>");
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete(nameof(RemoveAppointmentViaClient))]
        public async Task<IActionResult> RemoveAppointmentViaClient(string appointmentId)
        {
            var user = await _userManager.GetUserAsync(User);
            var appointment = await _appointmentService.GetFirsAsynct(x => x.Id == appointmentId && x.UserId == user.Id);

            if (appointment is null) return BadRequest("Not found.");

            var result = await _appointmentService.DeleteAsync(appointmentId); 
            if (!result.IsSuccess) return BadRequest(result.Message);

            var worker = await _baseUserService.FirstAsync(x => x.WorkerId == appointment.WorkerId);
            var service = await _serviceService.GetAsync(appointment.ServiceId);
            if (worker != null)
            {
                await _emailService.SendEmailAsync(worker.Email, "The client canceled the booking!", $"<label>" +
                    $"Client \"{user.UserName} {user.UserSurname}\" canceled the reservation for the \"{service.Name}\" service on the date " +
                    $"\"{appointment.Date.ToLocalTime().ToString("dd-MM-yyyy")} {appointment.TimeStart.ToString(@"hh\:mm")}\"" +
                    $"</label>");
            }

            return Ok();
        }

    }
}
