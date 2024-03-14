using BLL.Response;
using DAL.Repository;
using Domain.Models;

namespace BLL.Services
{
    /// <summary>
    /// Service class for managing company schedule exceptions.
    /// </summary>
    public class CompanyScheduleExceptionService : ServiceBase
    {
        private readonly CompanyScheduleExceptionRepository _scheduleExceptionRepository;
        private readonly CompanyRepository _companyRepository;

        /// <summary>
        /// Constructor for initializing the service with repositories.
        /// </summary>
        public CompanyScheduleExceptionService(CompanyScheduleExceptionRepository scheduleExceptionRepository, CompanyRepository companyRepository)
        {
            _scheduleExceptionRepository = scheduleExceptionRepository;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Retrieves company schedule exceptions asynchronously.
        /// </summary>
        /// <param name="companyId">The ID of the company to retrieve schedule exceptions for.</param>
        /// <returns>A collection of company schedule exceptions.</returns>

        public async Task<IEnumerable<CompanyScheduleException>> FindAsync(string compoanyId)
        {
            return await _scheduleExceptionRepository.GetAllFindAsync(x => x.CompanyId == compoanyId);
        }

        /// <summary>
        /// Adds a new company schedule exception asynchronously.
        /// </summary>
        /// <param name="model">The company schedule exception to add.</param>
        /// <returns>A service response indicating the outcome of the operation.</returns>
        public async Task<IServiceResponse> AddAsync(CompanyScheduleException model)
        {
            try
            {

                var company = await _companyRepository.GetAsync(model.CompanyId);
                if (company == null)
                    return BadResult("Company not found");

                var isCopy = await _scheduleExceptionRepository.AnyAsync(x => x.CompanyId == company.Id && x.ExceptionDate.Day == model.ExceptionDate.Day && x.ExceptionDate.Month == model.ExceptionDate.Month);

                if (isCopy) return BadResult("This exception already exists");

                await _scheduleExceptionRepository.InsertAsync(model);

                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }


        /// <summary>
        /// Deletes a company schedule exception asynchronously.
        /// </summary>
        /// <param name="id">The ID of the company schedule exception to delete.</param>
        /// <param name="companyId">The ID of the company that owns the schedule exception.</param>
        /// <returns>A service response indicating the outcome of the operation.</returns>
        public async Task<IServiceResponse> DeleteAsynce(string id, string  companyId)
        {

            try
            {
                var sE = await _scheduleExceptionRepository.GetAsync(id);

                if (sE == null)
                    return BadResult("Not found ");

                if (sE.CompanyId != companyId)
                    return BadResult("Sorry, you can't do that.");

                await _scheduleExceptionRepository.DeleteAsync(id);

                return OkResult();
            
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }

        }


    }
}
