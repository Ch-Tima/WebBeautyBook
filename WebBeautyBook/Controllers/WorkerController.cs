using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class WorkerController : ControllerBase
    {
        private readonly WorkerService _workerService;
        private readonly AssignmentService _assignmentService;
        private readonly AppointmentService _appointmentService;
        private readonly ReservationService _reservationService;
        private readonly CompanyOpenHoursService _companyOpenHoursService;

        public WorkerController(WorkerService workerService, AssignmentService assignmentService, AppointmentService appointmentService, ReservationService reservationService, CompanyOpenHoursService companyOpenHoursService)
        {
            _workerService = workerService;
            _assignmentService = assignmentService;;
            _appointmentService = appointmentService;
            _reservationService = reservationService;
            _companyOpenHoursService = companyOpenHoursService;
        }

        [HttpGet($"{nameof(GetWorkersByServiceId)}/{{serviceId}}")]
        public async Task<IEnumerable<Worker>> GetWorkersByServiceId(string serviceId)
        {
            var workers = await _workerService.GetAllIncludeFindAsync(x => x.Assignments.Any(r => r.ServiceId == serviceId));
            var result = await DeleteSensitiveData(workers);
            return result;
        }

        [HttpGet($"{nameof(GetWorkersByCompanyId)}/{{companyId}}")]
        public async Task<IEnumerable<Worker>> GetWorkersByCompanyId(string companyId)
        {
            var workers = await _workerService.GetAllIncludeFindAsync(x => x.CompanyId == companyId);
            var result = await DeleteSensitiveData(workers);
            return result;
        }

        [HttpGet(nameof(GetWorkersFreeTimeForService))]
        public async Task<IActionResult> GetWorkersFreeTimeForService([FromQuery]string workerId, [FromQuery] DateTime date, [FromQuery] string serviceId)
        {
            var assignments = await _assignmentService.FirstIncludeAsync(x => x.ServiceId == serviceId && x.WorkerId == workerId);

            if (assignments is null)
                return BadRequest("Not found.");

            var companyOpenHours = await _companyOpenHoursService.FindAsync(assignments.Worker.CompanyId);
            var openHours = companyOpenHours.FirstOrDefault(x => x.DayOfWeek == ((byte)date.DayOfWeek));
            if (openHours == null) 
                return BadRequest("We are not working on this day.");

            var reservationsInDay = await _reservationService.GetAllFindAsync(x => x.WorkerId == workerId && x.Date.Date == date.Date);
            var appointmentsInDay = await _appointmentService.GetAllFindAsync(x => x.WorkerId == workerId && x.Date.Date == date.Date);
            var interval = assignments.Service.Time;

            List<string> listFreeTime = new List<string>();
            TimeSpan currentTime = new TimeSpan(9, 0, 0);
            while (currentTime + interval <= new TimeSpan(20, 0, 0))
            {
                bool isAvailable = true;
                foreach (var appointment in appointmentsInDay)
                {
                    if (currentTime < appointment.TimeEnd && appointment.TimeStart < (currentTime + interval))
                    {
                        isAvailable = false;
                        break;
                    }
                }

                foreach (var reservation in reservationsInDay)
                {
                    if (currentTime < reservation.TimeEnd && reservation.TimeStart < (currentTime + interval))
                    {
                        isAvailable = false;
                        break;
                    }
                }

                if (isAvailable)
                {
                    listFreeTime.Add(currentTime.ToString());
                    currentTime = currentTime.Add(interval);
                }
                else currentTime = currentTime.Add(new TimeSpan(0, 5, 0));
            }
            return Ok(listFreeTime);
        }

        private async Task<IEnumerable<Worker>> DeleteSensitiveData(IEnumerable<Worker> workers)
        {
            workers.ToList().ForEach(worker =>
            {
                if(worker.BaseUser != null)
                {
                    worker.BaseUser.Email = String.Empty;
                    worker.BaseUser.PhoneNumber = String.Empty;
                    worker.BaseUser.NormalizedEmail = String.Empty;
                    worker.BaseUser.PasswordHash = String.Empty;
                    worker.BaseUser.ConcurrencyStamp = String.Empty;
                    worker.BaseUser.SecurityStamp = String.Empty;
                }
            });

            return workers;
        }

    }
}
