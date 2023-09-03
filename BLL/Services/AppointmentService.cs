using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;
        private readonly AssignmentRepository _assignmentRepository;
        private readonly ReservationRepository _reservationRepository;
        private readonly ServiceRepository _serviceRepository;

        public AppointmentService(AppointmentRepository appointmentRepository, AssignmentRepository assignmentRepository, ReservationRepository reservationRepository, ServiceRepository serviceRepository)
        {
            _appointmentRepository = appointmentRepository;
            _assignmentRepository = assignmentRepository;
            _reservationRepository = reservationRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<Appointment> GetAsync(string id)
        {
            return await _appointmentRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _appointmentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllFindAsync(Expression<Func<Appointment, bool>> func)
        {
            return await _appointmentRepository.GetAllFindAsync(func);
        }

        public async Task<ServiceResponse> InsertAsync(Appointment appointment)
        {
            try
            {
                var assignment = await _assignmentRepository.FirstIncludeAsync(x => x.WorkerId ==  appointment.WorkerId && x.ServiceId == appointment.ServiceId);
                if (assignment == null) return new ServiceResponse(false, "Not found service or worker.");

                appointment.TimeEnd = appointment.TimeStart + assignment.Service.Time;

                var appointments = await _appointmentRepository.GetAllFindAsync(a => a.Date.Date == appointment.Date.Date && a.WorkerId == appointment.WorkerId);
                var reservations = await _reservationRepository.GetAllFindAsync(a => a.Date.Date == appointment.Date.Date && a.WorkerId == appointment.WorkerId);
                if (HasOverlappingAppointments(appointments, appointment, reservations))
                    return new ServiceResponse(false, "Busy at this time.");


                await _appointmentRepository.InsertAsync(appointment);
                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task DeleteAsync(string id)
        {
            await _appointmentRepository.DeleteAsync(id);
        }

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
