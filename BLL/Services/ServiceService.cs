using DAL.Repository;
using BLL.Response;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    /// <summary>
    /// Service class responsible for managing service-related "<see cref="Service"/>" operations.
    /// </summary>
    public class ServiceService : ServiceBase
    {

        private readonly ServiceRepository _serviceRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly CompanyRepository _companyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceService"/> class.
        /// </summary>
        /// <param name="serviceRepository">The repository for services.</param>
        /// <param name="categoryRepository">The repository for categories.</param>
        /// <param name="companyRepository">The repository for companies.</param>
        public ServiceService(ServiceRepository serviceRepository, CategoryRepository categoryRepository, CompanyRepository companyRepository)
        {
            _serviceRepository = serviceRepository;
            _categoryRepository = categoryRepository;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Retrieves a service by its ID.
        /// </summary>
        /// <param name="id">The ID of the service to retrieve.</param>
        /// <returns>The retrieved service.</returns>
        public async Task<Service> GetAsync(string id) => await _serviceRepository.GetAsync(id);

        /// <summary>
        /// Retrieves all services.
        /// </summary>
        /// <returns>A collection of all services.</returns>
        public async Task<IEnumerable<Service>> GetAllAsync() => await _serviceRepository.GetAllAsync();

        /// <summary>
        /// Retrieves services based on a specified filter expression.
        /// </summary>
        /// <param name="expression">The filter expression to apply.</param>
        /// <returns>A collection of filtered services.</returns>

        public async Task<IEnumerable<Service>> GetAllFindAsync(Expression<Func<Service, bool>> expression) => await _serviceRepository.GetAllFindAsync(expression);

        /// <summary>
        /// Inserts a new service into the database.
        /// </summary>
        /// <param name="service">The service to insert.</param>
        /// <returns>A service response indicating the result of the operation.</returns>
        public async Task<IServiceResponse> InsertAsync(Service service)
        {
            try
            {
                //Check if the company exists
                if ((await _companyRepository.GetAsync(service.CompanyId)) == null) 
                    return BadResult($"We cannot find a company with id: {service.CompanyId}");

                //Check for duplicate service names
                var serviceCopy = await _serviceRepository.GetFirstAsync(x => x.Name.ToUpper() == service.Name.ToUpper() && x.CompanyId == service.CompanyId);
                if (serviceCopy != null) return BadResult($"Service named: {service.Name} exists.");

                //Check if the category exists
                if ((await _categoryRepository.GetAsync(service.CategoryId)) == null) 
                    return BadResult($"We cannot find a category with id: {service.CategoryId}");

                await _serviceRepository.InsertAsync(service);
                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a service by its ID.
        /// </summary>
        /// <param name="id">The ID of the service to delete.</param>
        public async Task DeleteAsync(string id)
        {
            await _serviceRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Updates an existing service.
        /// </summary>
        /// <param name="newService">The updated service.</param>
        /// <returns>A service response indicating the result of the operation.</returns>
        public async Task<IServiceResponse> UpdataAsync(Service newService)
        {
            try
            {
                //Check if the service exists
                var oldService = await _serviceRepository.GetAsync(newService.Id);
                if (oldService == null)
                    return BadResult($"We cannot find a Service with id: {newService.Id}.");

                //Check if the company exists
                if ((await _companyRepository.GetAsync(newService.CompanyId)) == null)
                    return BadResult($"We cannot find a Company with id: {newService.CompanyId}");

                //Check if this service is owned by this company
                if (oldService.CompanyId != newService.CompanyId)
                    return BadResult("This service is not owned by this company.");

                //Check for duplicate names
                var serviceCopy = await _serviceRepository.GetFirstAsync(x => 
                    x.Name.ToUpper() == newService.Name.ToUpper() && 
                    x.CompanyId == newService.CompanyId && 
                    x.Id != newService.Id
                );

                if (serviceCopy != null) return BadResult($"Service named: {newService.Name} exists.");

                await _serviceRepository.UpdateAsync(newService.Id, newService);
                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

    }
}
