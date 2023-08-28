using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentService(AppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
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

        public async Task InsertAsync(Appointment newRecord)
        {
            await _appointmentRepository.InsertAsync(newRecord);
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

    }
}
