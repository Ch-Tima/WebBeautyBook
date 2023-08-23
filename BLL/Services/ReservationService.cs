using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class ReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly WorkerRepository _workerRepository;

        public ReservationService(ReservationRepository reservationRepository, WorkerRepository workerRepository)
        {
            _reservationRepository = reservationRepository;
            _workerRepository = workerRepository;
        }

        public async Task<Reservation> GetAsync(string id)
        {
            return await _reservationRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _reservationRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllFindAsync(Expression<Func<Reservation, bool>> func)
        {
            return await _reservationRepository.GetAllFindAsync(func);
        }

        public async Task<ServiceResponse> InsertAsync(Reservation reservation)
        {
            try
            {
                var worker = await _workerRepository.GetAsync(reservation.WorkerId);

                if (worker == null) 
                    return new ServiceResponse(false, "Most likely you do not belong to any company.");

                if(reservation.TimeStart > reservation.TimeEnd)
                    (reservation.TimeStart, reservation.TimeEnd) = (reservation.TimeEnd, reservation.TimeStart);

                var reservations = await _reservationRepository.GetAllFindAsync(x => x.WorkerId == reservation.WorkerId && x.Date.Date == x.Date.Date);
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

        public async Task DeleteAsync(string id)
        {
            await _reservationRepository.DeleteAsync(id);
        }

        public async Task UpdataAsync(Reservation newReservation)
        {
            if (newReservation == null) return;

            await _reservationRepository.UpdateAsync(newReservation.Id, newReservation);
        }

        /// <summary>
        /// Searches the <paramref name="reservations"/> for an element that overlaps in time with <paramref name="item"/>
        /// </summary>
        /// <param name="reservations">The list in which we are looking for time intersections.</param>
        /// <param name="item">The element for which we are looking for intersections in time.</param>
        /// <returns>Returns true if present, otherwise false.</returns>
        private bool HasOverlappingReservations(List<Reservation> reservations, Reservation item)
        {
            foreach (var reservation in reservations)
                if (reservation.Date.Date == item.Date.Date)
                    if ((item.TimeStart < reservation.TimeEnd && item.TimeEnd > reservation.TimeStart) ||
                        (reservation.TimeStart < item.TimeEnd && reservation.TimeEnd > item.TimeStart))
                        return true;//Overlapping reservation detected

            return false;//No overlapping reservation found.
        }

    }
}
