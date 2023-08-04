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
        private readonly CategoryRepository _categoryRepository;
        private readonly CompanyRepository _companyRepository;

        public ServiceService(ServiceRepository serviceRepository, CategoryRepository categoryRepository, CompanyRepository companyRepository)
        {
            _serviceRepository = serviceRepository;
            _categoryRepository = categoryRepository;
            _companyRepository = companyRepository;
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
            return await _serviceRepository.GetAllFindAsync(expression); ;
        }

        public async Task<ServiceResponse> InsertAsync(Service service)
        {
            try
            {
                //Is exist company
                if ((await _companyRepository.GetAsync(service.CompanyId)) == null) 
                    return new ServiceResponse(false, $"We cannot find a company with id: {service.CompanyId}");

                //Check for duplicate service names
                var serviceCopy = await _serviceRepository.GetFirstAsync(x => x.Name.ToUpper() == service.Name.ToUpper() && x.CompanyId == service.CompanyId);
                if (serviceCopy != null) return new ServiceResponse(false, $"Service named: {service.Name} exists.");

                //Is exist category
                if ((await _categoryRepository.GetAsync(service.CategoryId)) == null) 
                    return new ServiceResponse(false, $"We cannot find a category with id: {service.CategoryId}");

                await _serviceRepository.InsertAsync(service);

                return new ServiceResponse(true, "Ok");

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task DeleteAsync(string id)
        {
            await _serviceRepository.DeleteAsync(id);
        }

        public async Task<ServiceResponse> UpdataAsync(Service newService)
        {
            try
            {
                //Is exist service
                var oldService = await _serviceRepository.GetAsync(newService.Id);
                if (oldService == null)
                    return new ServiceResponse(false, $"We cannot find a Service with id: {newService.Id}.");

                //Is exist company
                if ((await _companyRepository.GetAsync(newService.CompanyId)) == null)
                    return new ServiceResponse(false, $"We cannot find a Company with id: {newService.CompanyId}");

                //Is this service owned by this company
                if (oldService.CompanyId != newService.CompanyId)
                    return new ServiceResponse(false, "This service is not owned by this company.");

                //Check for duplicate names
                var serviceCopy = await _serviceRepository.GetFirstAsync(x => 
                    x.Name.ToUpper() == newService.Name.ToUpper() && 
                    x.CompanyId == newService.CompanyId && 
                    x.Id != newService.Id
                );

                if (serviceCopy != null) return new ServiceResponse(false, $"Service named: {newService.Name} exists.");

                await _serviceRepository.UpdateAsync(newService.Id, newService);

                return new ServiceResponse(true, "Ok");

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

    }
}
