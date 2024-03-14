using BLL.Response;
using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CompanyService : ServiceBase
    {
        private readonly CompanyRepository _companyRepository;
        private readonly WorkerService _workerService;
        private readonly ServiceRepository _serviceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyService"/> class.
        /// </summary>
        /// <param name="companyRepository">The repository for companies.</param>
        /// <param name="workerService">The service for managing workers.</param>
        /// <param name="serviceRepository">The repository for services.</param>
        public CompanyService(CompanyRepository companyRepository, WorkerService workerService, ServiceRepository serviceRepository)
        {
            _companyRepository = companyRepository;
            _workerService = workerService;
            _serviceRepository = serviceRepository;
        }

        /// <summary>
        /// Retrieves a company by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the company to retrieve.</param>
        /// <returns>The company or null if not found.</returns
        public async Task<Company> GetAsync(string id) => await _companyRepository.GetAsync(id);

        /// <summary>
        /// This method is intended for public use in the "authorization not required" API.
        /// </summary>
        /// <param name="id">Company primary key</param>
        /// <returns><see cref="Company"/> with navigation fields.</returns>
        public async Task<Company> GetIncludeForClientAsync(string id)
        {
            var company = await _companyRepository.GetIncludeAsync(id);
            company.Services = (await _serviceRepository.GetAllFindAsync(x => x.Assignments.Count > 0 && x.CompanyId == id)).ToList();
            return company;
        }

        /// <summary>
        /// This method is the opposite of <see cref="GetIncludeForClientAsync(string)"/> method, it has no limitation.
        /// </summary>
        /// <param name="id">Company primary key</param>
        /// <returns><see cref="Company"/> with navigation fields.</returns>
        public async Task<Company> GetIncludeForCompanyAsync(string id) => await _companyRepository.GetIncludeAsync(id);

        /// <summary>
        /// Retrieves all companies asynchronously.
        /// </summary>
        /// <returns>A collection of companies.</return
        public async Task<IEnumerable<Company>> GetAllAsync() => await _companyRepository.GetAllAsync();

        //TODO
        /// <summary>
        /// Retrieves the top ten companies based on an optional location filter.
        /// </summary>
        /// <param name="location">The location filter (optional).</param>
        /// <returns>The top ten companies.</returns>
        public async Task<IEnumerable<Company>> GetTopTen(string? location)
        {
            return (await _companyRepository.GetFindIncludeAsync(x => true)).Take(10);
        }

        /// <summary>
        /// Retrieves a collection of companies that match the specified criteria, including related entities.
        /// </summary>
        /// <param name="expression">The filter expression to apply to the query.</param>
        /// <returns>A collection of companies that match the specified criteria.</returns>
        public async Task<IEnumerable<Company>> GetFindIncludeAsync(Expression<Func<Company, bool>> expression)
        {
            return (await _companyRepository.GetFindIncludeAsync(expression));
        }

        /// <summary>
        /// Retrieves all companies that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="expression">The filter criteria.</param>
        /// <returns>A collection of companies that match the criteria.</returns>
        public async Task<IEnumerable<Company>> GetAllFindAsync(Expression<Func<Company, bool>> expression)
        {
            return await _companyRepository.GetAllFindAsync(expression);
        }

        /// <summary>
        /// Creates a new company and associates it with a user.
        /// </summary>
        /// <param name="company">The company to create.</param>
        /// <param name="own">The user who owns the company.</param>
        /// <returns>A <see cref="IServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<IServiceResponse> CreateCompany(Company company, BaseUser own)
        {
            try
            {
                //create company
                await _companyRepository.InsertAsync(company);
                //create Worker for own
                await _workerService.InsertAsync(company.Id, own);

                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing company asynchronously.
        /// </summary>
        /// <param name="company">The updated company.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Update(Company company)
        {
            await _companyRepository.UpdateAsync(company.Id, company);
        }

    }
}
