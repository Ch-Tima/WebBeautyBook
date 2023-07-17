using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class ScheduleService
    {
        private readonly ScheduleRepository _repositorySchedule;
        public ScheduleService(ScheduleRepository repository)
        {
            _repositorySchedule = repository;
        }

        public async Task<Schedule> GetAsync(string id)
        {
            return await _repositorySchedule.GetAsync(id);
        }

        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            return await _repositorySchedule.GetAllAsync();
        }

        public async Task<IEnumerable<Schedule>> GetAllFindAsync(Expression<Func<Schedule, bool>> func)
        {
            return await _repositorySchedule.GetAllFindAsync(func);
        }

        public async Task InsertAsync(Schedule schedule)
        {
            await _repositorySchedule.InsertAsync(schedule);
        }

        public async Task DeleteAsync(string id)
        {
            await _repositorySchedule.DeleteAsync(id);
        }

        public async Task UpdataAsync(Schedule newSchedule)
        {
            if (newSchedule == null) return;

            await _repositorySchedule.UpdateAsync(newSchedule.Id, newSchedule);
        }

    }
}
