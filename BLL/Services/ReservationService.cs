using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class ReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        public ReservationService(ReservationRepository repository)
        {
            _reservationRepository = repository;
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

        public async Task InsertAsync(Reservation reservation)
        {
            await _reservationRepository.InsertAsync(reservation);
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

    }
}
