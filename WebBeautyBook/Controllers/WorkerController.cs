using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            // Fetch assignments for the specified worker and
            var assignments = await _assignmentService.FirstIncludeAsync(x => x.ServiceId == serviceId && x.WorkerId == workerId);

            // Check if assignments are found
            if (assignments is null)
                return BadRequest("Not found.");

            // Fetch the company's open hours for the specified day of the week
            var companyOpenHours = await _companyOpenHoursService.FindAsync(assignments.Worker.CompanyId);
            var openHours = companyOpenHours.FirstOrDefault(x => x.DayOfWeek == ((byte)date.DayOfWeek));
            if (openHours == null) // Check if the company is open on the specified day
                return BadRequest("We are not working on this day.");

            // Fetch reservations and appointments for the specified worker and date
            var reservationsInDay = await _reservationService.GetAllFindAsync(x => x.WorkerId == workerId && x.Date.Date == date.Date);
            var appointmentsInDay = await _appointmentService.GetAllFindAsync(x => x.WorkerId == workerId && x.Date.Date == date.Date);
            var interval = assignments.Service.Time;

            List<string> listFreeTime = new List<string>();// Time interval for the service
            TimeSpan currentTime = new TimeSpan(9, 0, 0);// Start with the opening time of the company
            TimeSpan endTime = new TimeSpan(20, 0, 0);// Assuming company closes at 20:00
            while (currentTime + interval <= endTime)
            {
                bool isAvailable = IsTimeSlotAvailable(appointmentsInDay, reservationsInDay, currentTime, interval);

                if (isAvailable)// If the time slot is available, add it to the list of free times
                {
                    listFreeTime.Add(currentTime.ToString());
                    currentTime = currentTime.Add(interval);
                }// If not available, move to the next 5-minute interval
                else currentTime = currentTime.Add(new TimeSpan(0, 5, 0));
            }
            return Ok(listFreeTime);// Return the list of available time slots
        }

        /// <summary>
        /// Deletes sensitive data (such as email, phone number, password hash, and security stamps) from a collection of workers.
        /// </summary>
        /// <param name="workers">The collection of workers whose sensitive data needs to be deleted.</param>
        /// <returns>The same collection of workers with sensitive data cleared.</returns>
        private async Task<IEnumerable<Worker>> DeleteSensitiveData(IEnumerable<Worker> workers)
        {
            //Iterate through each worker in the collection
            workers.ToList().ForEach(worker =>
            {
                //Clear sensitive data fields of the associated BaseUser
                if (worker.BaseUser != null)
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

        /// <summary>
        /// Checks if a time slot is available by verifying that it does not overlap with existing appointments or reservations.
        /// </summary>
        /// <param name="appointments">The collection of existing appointments.</param>
        /// <param name="reservations">The collection of existing reservations.</param>
        /// <param name="startTime">The start time of the time slot to check.</param>
        /// <param name="interval">The time interval of the time slot to check.</param>
        /// <returns><c>true</c> if the time slot is available; otherwise, <c>false</c>.</returns>
        private bool IsTimeSlotAvailable(IEnumerable<Appointment> appointments, IEnumerable<Reservation> reservations, TimeSpan startTime, TimeSpan interval)
        {
            foreach (var appointment in appointments)// Check if the current time slot overlaps with an existing appointment
                if (startTime < appointment.TimeEnd && appointment.TimeStart < (startTime + interval))
                    return false;// Time slot is not available

            foreach (var reservation in reservations)// Check if the current time slot overlaps with an existing reservation
                if (startTime < reservation.TimeEnd && reservation.TimeStart < (startTime + interval))
                    return false;// Time slot is not available

            return true;
        }

    }
}
