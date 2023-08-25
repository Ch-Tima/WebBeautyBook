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
    [Authorize(Roles = $"{Roles.OWN_COMPANY}, {Roles.MANAGER}, {Roles.WORKER}")]
    public class ReservationController : ControllerBase
    {

        private readonly ReservationService _reservationService;
        private readonly UserManager<BaseUser> _userManager;

        public ReservationController(ReservationService reservationService, UserManager<BaseUser> userManager)
        {
            _reservationService = reservationService;
            _userManager = userManager;
        }

        [HttpGet(nameof(GetMy))]
        public async Task<IEnumerable<Reservation>> GetMy()
        {
            var user = await _userManager.GetUserAsync(User);
            return await _reservationService.GetAllFindAsync(x => x.WorkerId == user.WorkerId);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] ReservationViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var reservation = new Reservation()
            {
                Date = viewModel.Date.LocalDateTime,
                TimeStart = viewModel.TimeStart.ToTimeSpan(),
                TimeEnd = viewModel.TimeEnd.ToTimeSpan(),
                Description = viewModel.Description,
                WorkerId = user.WorkerId,
            };

            var result = await _reservationService.InsertAsync(reservation);

            if(!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] ReservationViewModel viewModel, [FromQuery] string id)
        {
            var user = await _userManager.GetUserAsync(User);

            var result = await _reservationService.UpdataAsync(new Reservation()
            {
                Id = id,
                WorkerId = user.WorkerId,
                Description = viewModel.Description,
                Date = viewModel.Date.ToLocalTime().DateTime,
                TimeStart= viewModel.TimeStart.ToTimeSpan(),
                TimeEnd = viewModel.TimeEnd.ToTimeSpan(),
            });

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();
        }

    }
}
