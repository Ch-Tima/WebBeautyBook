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
        private readonly WorkerService _workerService;

        public ReservationController(ReservationService reservationService, UserManager<BaseUser> userManager, WorkerService workerService)
        {
            _reservationService = reservationService;
            _userManager = userManager;
            _workerService = workerService;
        }

        [HttpGet(nameof(GetMy))]
        public async Task<IEnumerable<Reservation>> GetMy()
        {
            var user = await _userManager.GetUserAsync(User);
            return await _reservationService.GetAllFindAsync(x => x.WorkerId == user.WorkerId);
        }

        [HttpGet(nameof(Filter))]
        public async Task<IEnumerable<Reservation>> Filter([FromQuery] string[] ids)
        {
            var user = await _userManager.GetUserAsync(User);
            var worker = await _workerService.GetAsync(user.WorkerId);

            if (worker == null)
                return new List<Reservation>();

            return await _reservationService.GetAllFindAsync(r => r.Worker.CompanyId == worker.CompanyId && ids.Contains(r.WorkerId));
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
            var reservation = await _reservationService.GetAsync(id);
            if(reservation == null) return BadRequest("Not found Reservation.");
            if (reservation.WorkerId != user.WorkerId)
                return BadRequest("This is not your Reservation.");


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

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var reservation = await _reservationService.GetAsync(id);
            if (reservation.WorkerId != user.WorkerId)
                return BadRequest("This is not your Reservation.");

            var result = await _reservationService.DeleteAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();

        }

    }
}
