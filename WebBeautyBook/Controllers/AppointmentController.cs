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
        private readonly UserManager<BaseUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AppointmentController(AppointmentService appointmentService, UserManager<BaseUser> userManager, IEmailSender emailSender)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet(nameof(GetMyAppointments))]
        [Authorize(Roles = $"{Roles.CLIENT}")]
        public async Task GetMyAppointments()
        {

        }

        [HttpGet(nameof(GetCompanyAppointments))]
        [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.WORKER}")]
        public async Task GetCompanyAppointments()
        {

        }

        [Authorize(Roles = $"{Roles.CLIENT}")]
        [HttpPut(nameof(CreateAppointmentViaClient))]
        public async Task<IActionResult> CreateAppointmentViaClient([FromBody] AppointmentViewModel appointmentViewModel)
        {
            return Ok();
        }

    }
}
