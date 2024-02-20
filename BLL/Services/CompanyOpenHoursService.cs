using DAL.Repository;
using Domain.Models;

namespace BLL.Services
{
    public class CompanyOpenHoursService
    {
        private readonly CompanyOpenHoursRepository _companyOpenHoursRepository;
        private readonly CompanyRepository _companyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyOpenHoursService"/> class.
        /// </summary>
        /// <param name="companyOpenHoursRepository">The repository for company open hours.</param>
        /// <param name="companyRepository">The repository for companies.</param>
        public CompanyOpenHoursService(CompanyOpenHoursRepository companyOpenHoursRepository, CompanyRepository companyRepository)
        {
            _companyOpenHoursRepository = companyOpenHoursRepository;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Find company open hours by company ID.
        /// </summary>
        /// <param name="companyId">The ID of the company to retrieve open hours for.</param>
        /// <returns>A collection of company open hours.</returns>
        public async Task<IEnumerable<CompanyOpenHours>> FindAsync(string companyId)
        {
            return await _companyOpenHoursRepository.GetAllFindAsync(x => x.CompanyId == companyId);
        }

        /// <summary>
        /// Add company open hours for a specific day of the week.
        /// </summary>
        /// <param name="companyOpenHours">The open hours to add.</param>
        /// <param name="companyId">The ID of the company to add open hours for.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<ServiceResponse> AddAsync(CompanyOpenHours companyOpenHours, string companyId)
        {
            try
            {
                if (companyOpenHours.DayOfWeek < 0 || companyOpenHours.DayOfWeek > 7)
                    return new ServiceResponse(false, "There is no such day");

                if (companyId is null)
                    return new ServiceResponse(false, "Company ID cannot be null");

                var company = await _companyRepository.GetAsync(companyId);
                if (company is null)
                    return new ServiceResponse(false, "Not found company");

                var findDuplicate = _companyOpenHoursRepository.GetFirstAsync(x => x.DayOfWeek == companyOpenHours.DayOfWeek && x.CompanyId == companyId);
                if (findDuplicate != null)
                    return new ServiceResponse(false, "There is a schedule for the day");

                companyOpenHours.CompanyId = companyId;
                await _companyOpenHoursRepository.InsertAsync(companyOpenHours);

                return new ServiceResponse(true, "OK");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task<ServiceResponse> DeleteAsynce(string id)
        {
            try
            {
                var item = await _companyOpenHoursRepository.GetFirstAsync(x => x.Id ==  id);

                if (item == null) return new ServiceResponse(false, "Not found");

                await _companyOpenHoursRepository.DeleteAsync(id);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task<ServiceResponse> UpdateHoursAsync(string id, TimeSpan openFrom, TimeSpan openUntil)
        {
            try
            {
                var item = await _companyOpenHoursRepository.GetFirstAsync(x => x.Id == id);
                if (item is null) return new ServiceResponse(false, "Not Found");

                if(openFrom > openUntil) (openFrom, openUntil) = (openUntil, openFrom);

                item.OpenFrom = openFrom;
                item.OpenUntil = openUntil;

                await _companyOpenHoursRepository.UpdateAsync(id, item);
                return new ServiceResponse(true, "OK");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

    }
}