using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    /// <summary>
    /// I know this class name is very stupid.
    /// </summary>
    public class ServiceService
    {

        private readonly ServiceRepository _serviceRepository;

        public ServiceService(ServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Service> GetAsync(string id)
        {
            return await _serviceRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _serviceRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Service>> GetAllFindAsync(Expression<Func<Service, bool>> expression)
        {
            return await _serviceRepository.GetAllFindAsync(expression);
        }

        public async Task InsertAsync(Service service)
        {
            await _serviceRepository.InsertAsync(service);
        }

        public async Task DeleteAsync(string id)
        {
            await _serviceRepository.DeleteAsync(id);
        }

        public async Task UpdataAsync(Service newService)
        {
            if (newService == null) return;

            await _serviceRepository.UpdateAsync(newService.Id, newService);
        }

    }
}
