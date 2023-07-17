using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class RecordService
    {
        private readonly RecordRepository _recordRepository;

        public RecordService(RecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<Record> GetAsync(string id)
        {
            return await _recordRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Record>> GetAllAsync()
        {
            return await _recordRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Record>> GetAllFindAsync(Expression<Func<Record, bool>> func)
        {
            return await _recordRepository.GetAllFindAsync(func);
        }

        public async Task InsertAsync(Record newRecord)
        {
            await _recordRepository.InsertAsync(newRecord);
        }

        public async Task DeleteAsync(string id)
        {
            await _recordRepository.DeleteAsync(id);
        }

        public async Task UpdataAsync(Record newRecord)
        {
            if (newRecord == null) return;

            await _recordRepository.UpdateAsync(newRecord.Id, newRecord);
        }

    }
}
