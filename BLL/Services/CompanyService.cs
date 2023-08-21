using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CompanyService
    {
        private readonly CompanyRepository _companyRepository;
        private readonly WorkerService _workerService;
        private readonly ServiceRepository _serviceRepository;

        public CompanyService(CompanyRepository companyRepository, WorkerService workerService, ServiceRepository serviceRepository)
        {
            _companyRepository = companyRepository;
            _workerService = workerService;
            _serviceRepository = serviceRepository;
        }

        public async Task<Company> GetAsync(string id)
        {
            return await _companyRepository.GetAsync(id);
        }

        /// <summary>
        /// This method is intended for public use in the "authorization not required" API.
        /// </summary>
        /// <param name="id">Company primary key</param>
        /// <returns><see cref="Company"/> with navigation fields.</returns>
        public async Task<Company> GetIncludeForClientAsync(string id)
        {
            var company = await _companyRepository.GetIncludeAsync(id);
            company.Services = (await _serviceRepository.GetAllFindAsync(x => x.Assignments.Count > 0)).ToList();
            return company;
        }

        /// <summary>
        /// This method is the opposite of <see cref="GetIncludeForClientAsync(string)"/> method, it has no limitation.
        /// </summary>
        /// <param name="id">Company primary key</param>
        /// <returns><see cref="Company"/> with navigation fields.</returns>
        public async Task<Company> GetIncludeForCompanyAsync(string id)
        {
            return await _companyRepository.GetIncludeAsync(id);
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Company>> GetAllIncludeAsync()
        {
            return await _companyRepository.GetAllIncludeAsync();
        }

        /// <summary>
        /// ! TODO !
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Company>> GetTopTen(string? location)
        {
            return (await _companyRepository.GetFindIncludeAsync(x => true)).Take(10);
        }

        public async Task<Company> GetFirstAsync(Expression<Func<Company, bool>> expression)
        {
            return await _companyRepository.GetFirstAsync(expression);
        }

        public async Task<IEnumerable<Company>> GetAllFindAsync(Expression<Func<Company, bool>> expression)
        {
            return await _companyRepository.GetAllFindAsync(expression);
        }

        public async Task CreateCompany(Company company, BaseUser own)
        {
            //create company
            await _companyRepository.InsertAsync(company);

            //create Worker for own
            await _workerService.InsertAsync(company.Id, own);
        }

        public async Task Update(Company company)
        {
            await _companyRepository.UpdateAsync(company.Id, company);
        }

    }
}
