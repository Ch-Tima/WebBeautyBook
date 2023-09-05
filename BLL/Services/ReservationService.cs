using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class ReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly WorkerRepository _workerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationService"/> class.
        /// </summary>
        /// <param name="reservationRepository">The repository for reservations.</param>
        /// <param name="workerRepository">The repository for workers.</param>
        public ReservationService(ReservationRepository reservationRepository, WorkerRepository workerRepository)
        {
            _reservationRepository = reservationRepository;
            _workerRepository = workerRepository;
        }

        /// <summary>
        /// <see cref="Reservation"/> a reservation by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>The reservation or null if not found.</returns>
        public async Task<Reservation> GetAsync(string id) => await _reservationRepository.GetAsync(id);

        /// <summary>
        /// <see cref="Reservation"/> all reservations asynchronously.
        /// </summary>
        /// <returns>A collection of reservations.</returns
        public async Task<IEnumerable<Reservation>> GetAllAsync() => await _reservationRepository.GetAllAsync();

        /// <summary>
        /// <see cref="Reservation"/> all reservations that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="func">The filter criteria.</param>
        /// <returns>A collection of reservations that match the criteria.</returns>
        public async Task<IEnumerable<Reservation>> GetAllFindAsync(Expression<Func<Reservation, bool>> func) => await _reservationRepository.GetAllFindAsync(func);

        /// <summary>
        /// Inserts a new reservation asynchronously.
        /// </summary>
        /// <param name="reservation">The reservation to insert.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the insertion.</returns>
        public async Task<ServiceResponse> InsertAsync(Reservation reservation)
        {
            try
            {
                //find worker profile
                var worker = await _workerRepository.GetAsync(reservation.WorkerId);
                if (worker == null) 
                    return new ServiceResponse(false, "Most likely you do not belong to any company.");

                //swap, if TimeEnd more than TimeStart
                if (reservation.TimeStart > reservation.TimeEnd)
                    (reservation.TimeStart, reservation.TimeEnd) = (reservation.TimeEnd, reservation.TimeStart);

                //search for intersections in time
                var reservations = await _reservationRepository.GetAllFindAsync(x => x.WorkerId == reservation.WorkerId && x.Date.Date == reservation.Date.Date);
                if (reservations != null && HasOverlappingReservations(reservations.ToList(), reservation))
                    return new ServiceResponse(false, "The Reservation cannot overlap with another reservation.");

                await _reservationRepository.InsertAsync(reservation);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a reservation by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the deletion.</returns>
        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            try
            {
                if (!await IsExistAsync(id)) return new ServiceResponse(false, "Not found reservation.");

                await _reservationRepository.DeleteAsync(id);
                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing reservation asynchronously.
        /// </summary>
        /// <param name="reservation">The updated reservation.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the update.</returns>
        public async Task<ServiceResponse> UpdataAsync(Reservation reservation)
        {
            try
            {
                //find worker profile
                var worker = await _workerRepository.GetAsync(reservation.WorkerId);
                if (worker == null)
                    return new ServiceResponse(false, "Most likely you do not belong to any company.");

                //find reservation
                if (!await IsExistAsync(reservation.Id))
                    return new ServiceResponse(false, "Not found reservation.");

                //swap, if TimeEnd more than TimeStart
                if (reservation.TimeStart > reservation.TimeEnd)
                    (reservation.TimeStart, reservation.TimeEnd) = (reservation.TimeEnd, reservation.TimeStart);

                //search for intersections in time
                var reservations = await _reservationRepository.GetAllFindAsync(x => x.WorkerId == reservation.WorkerId && x.Date.Date == reservation.Date.Date && x.Id != reservation.Id);
                if (reservations != null && HasOverlappingReservations(reservations.ToList(), reservation))
                    return new ServiceResponse(false, "The Reservation cannot overlap with another reservation.");

                await _reservationRepository.UpdateAsync(reservation.Id, reservation);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        /// <summary>
        /// Searches the <paramref name="reservations"/> for an element that overlaps in time with <paramref name="item"/>
        /// </summary>
        /// <param name="reservations">The list in which we are looking for time intersections.</param>
        /// <param name="item">The element for which we are looking for intersections in time.</param>
        /// <returns>Returns true if present, otherwise false.</returns>
        private bool HasOverlappingReservations(IEnumerable<Reservation> reservations, Reservation item, IEnumerable<Appointment>? appointments = null)
        {
            foreach (var reservation in reservations)
                if (reservation.Date.Date == item.Date.Date)
                    if ((item.TimeStart < reservation.TimeEnd && item.TimeEnd > reservation.TimeStart) ||
                        (reservation.TimeStart < item.TimeEnd && reservation.TimeEnd > item.TimeStart))
                        return true;//Overlapping reservation detected


            if (appointments == null || appointments.Count() == 0) return false;

            foreach (var appointment in appointments)
                if (appointment.Date.Date == item.Date.Date)
                    if ((item.TimeStart < appointment.TimeEnd && item.TimeEnd > appointment.TimeStart) ||
                        (appointment.TimeStart < item.TimeEnd && appointment.TimeEnd > item.TimeStart))
                        return true;//Overlapping reservation detected

            return false;//No overlapping reservation found.
        }

        /// <summary>
        /// Searches for the element with the given <paramref name="id"/> and returns bool values.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns true if the element is found, false otherwise.</returns>
        private async Task<bool> IsExistAsync(string id)
        {
            if(id == null) 
                return false;

            var item = await _reservationRepository.GetAsync(id);
            if (item == null) 
                return false;

            return true;
        }

    }
}
