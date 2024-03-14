using BLL.Response;
using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class AppointmentService : ServiceBase
    {
        private readonly AppointmentRepository _appointmentRepository;
        private readonly AssignmentRepository _assignmentRepository;
        private readonly ReservationRepository _reservationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentService"/> class.
        /// </summary>
        /// <param name="appointmentRepository">The repository for appointment data.</param>
        /// <param name="assignmentRepository">The repository for assignment data.</param>
        /// <param name="reservationRepository">The repository for reservation data.</param>
        /// <param name="serviceRepository">The repository for service data.</param>
        public AppointmentService(AppointmentRepository appointmentRepository, AssignmentRepository assignmentRepository, ReservationRepository reservationRepository)
        {
            _appointmentRepository = appointmentRepository;
            _assignmentRepository = assignmentRepository;
            _reservationRepository = reservationRepository;
        }

        /// <summary>
        /// Get an appointment by its ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to retrieve.</param>
        /// <returns>The requested appointment.</returns>
        public async Task<Appointment> GetAsync(string id)
        {
            return await _appointmentRepository.GetAsync(id);
        }

        /// <summary>
        /// Asynchronously retrieves the first appointment that matches a specified filtering expression.
        /// </summary>
        /// <param name="func">The filtering expression to apply to the appointments.</param>
        /// <returns>
        /// An asynchronous task that represents the operation. The task result contains the first appointment
        /// that matches the specified filter, or null if no matching appointment is found.
        /// </returns>
        public async Task<Appointment?> GetFirsAsynct(Expression<Func<Appointment, bool>> func) => await _appointmentRepository.GetFirstAsync(func);

        /// <summary>
        /// Get all appointments.
        /// </summary>
        /// <returns>A collection of all appointments.</returns>
        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _appointmentRepository.GetAllAsync();
        }

        /// <summary>
        /// Get appointments that match the specified filter expression.
        /// </summary>
        /// <param name="func">The filter expression to match appointments.</param>
        /// <returns>A collection of matching appointments.</returns>
        public async Task<IEnumerable<Appointment>> GetAllFindAsync(Expression<Func<Appointment, bool>> func)
        {
            return await _appointmentRepository.GetAllFindAsync(func);
        }

        public async Task<IEnumerable<Appointment>> GetAllFindIncludeAsync(Expression<Func<Appointment, bool>> func) => await _appointmentRepository.GetAllFindIncludeAsync(func);

        /// <summary>
        /// Inserts a new appointment into the repository.
        /// </summary>
        /// <param name="appointment">The appointment to insert.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the insertion operation.</returns>
        public async Task<IServiceResponse> InsertAsync(Appointment appointment)
        {
            try
            {
                var assignment = await _assignmentRepository.FirstIncludeAsync(x => x.WorkerId ==  appointment.WorkerId && x.ServiceId == appointment.ServiceId);
                if (assignment == null) return BadResult("Not found service or worker.");

                appointment.TimeEnd = appointment.TimeStart + assignment.Service.Time;

                var appointments = await _appointmentRepository.GetAllFindAsync(a => a.Date.Date == appointment.Date.Date && a.WorkerId == appointment.WorkerId);
                var reservations = await _reservationRepository.GetAllFindAsync(a => a.Date.Date == appointment.Date.Date && a.WorkerId == appointment.WorkerId);
                if (HasOverlappingAppointments(appointments, appointment, reservations))
                    return BadResult("Busy at this time.");


                await _appointmentRepository.InsertAsync(appointment);
                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an appointment by its ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to delete.</param>
        public async Task<IServiceResponse> DeleteAsync(string id)
        {
            try
            {
                await _appointmentRepository.DeleteAsync(id);
                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing appointment with new data.
        /// </summary>
        /// <param name="newRecord">The updated appointment data.</param>
        public async Task UpdataAsync(Appointment newRecord)
        {
            if (newRecord == null) return;

            await _appointmentRepository.UpdateAsync(newRecord.Id, newRecord);
        }

        /// <summary>
        /// Searches the <paramref name="appointments"/> for an element that overlaps in time with <paramref name="item"/>
        /// </summary>
        /// <param name="appointments">The list in which we are looking for time intersections.</param>
        /// <param name="item">The element for which we are looking for intersections in time.</param>
        /// <returns>Returns true if present, otherwise false.</returns>
        private bool HasOverlappingAppointments(IEnumerable<Appointment> appointments, Appointment item, IEnumerable<Reservation>? reservations = null)
        {
            foreach (var appointment in appointments)
                if (appointment.Date.Date == item.Date.Date)
                    if ((item.TimeStart < appointment.TimeEnd && item.TimeEnd > appointment.TimeStart) ||
                        (appointment.TimeStart < item.TimeEnd && appointment.TimeEnd > item.TimeStart))
                        return true;//Overlapping reservation detected

            if(reservations == null || reservations.Count() == 0) return false;

            foreach (var reservation in reservations)
                if (reservation.Date.Date == item.Date.Date)
                    if ((item.TimeStart < reservation.TimeEnd && item.TimeEnd > reservation.TimeStart) ||
                        (reservation.TimeStart < item.TimeEnd && reservation.TimeEnd > item.TimeStart))
                        return true;//Overlapping reservation detected

            return false;//No overlapping reservation found.
        }

    }
}
