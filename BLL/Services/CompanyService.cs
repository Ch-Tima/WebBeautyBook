using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CompanyService
    {
        private readonly CompanyRepository _companyRepository;
        private readonly WorkerService _workerService;
        public CompanyService(CompanyRepository companyRepository, WorkerService workerService)
        {
            _companyRepository = companyRepository;
            _workerService = workerService;
        }

        public async Task<Company> GetAsync(string id)
        {
            return await _companyRepository.GetAsync(id);
        }

        public async Task<Company> GetIncludeAsync(string id)
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
