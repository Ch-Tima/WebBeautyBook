using DAL.Repository;
using Domain.Models;

namespace BLL.Services
{
    /// <summary>
    /// Service for managing company open hours.
    /// </summary>
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
        public async Task<ServiceResponse> AddAsync(CompanyOpenHours companyOpenHours)
        {
            try
            {
                // Check if the day of the week is valid.
                if (companyOpenHours.DayOfWeek < 0 || companyOpenHours.DayOfWeek > 7)
                    return new ServiceResponse(false, "There is no such day");

                // Check if the company ID is null.
                if (companyOpenHours.CompanyId is null)
                    return new ServiceResponse(false, "Company ID cannot be null");

                // Retrieve the company by its ID.
                var company = await _companyRepository.GetAsync(companyOpenHours.CompanyId);
                if (company is null)
                    return new ServiceResponse(false, "Not found company");

                // Check for duplicate open hours for the same day and company.
                var findDuplicate = await _companyOpenHoursRepository.GetFirstAsync(x => x.DayOfWeek == companyOpenHours.DayOfWeek && x.CompanyId == companyOpenHours.CompanyId);
                if (findDuplicate is not null)
                    return new ServiceResponse(false, "There is a schedule for the day");

                // Ensure the open from time is earlier than the open until time.
                if (companyOpenHours.OpenFrom > companyOpenHours.OpenUntil) 
                    (companyOpenHours.OpenFrom, companyOpenHours.OpenUntil) = (companyOpenHours.OpenUntil, companyOpenHours.OpenFrom);

                // Insert the new open hours into the repository.
                await _companyOpenHoursRepository.InsertAsync(companyOpenHours);

                return new ServiceResponse(true, "OK");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        /// <summary>
        /// Delete company open hours by ID.
        /// </summary>
        /// <param name="id">The ID of the company open hours to delete.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<ServiceResponse> DeleteAsynce(string id)
        {
            try
            {
                // Find the company open hours by ID.
                var item = await _companyOpenHoursRepository.GetFirstAsync(x => x.Id ==  id);

                // If the item is not found, return a corresponding error response.
                if (item == null) return new ServiceResponse(false, "Not found");

                // Delete the company open hours from the repository.
                await _companyOpenHoursRepository.DeleteAsync(id);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        /// <summary>
        /// Update company open hours for a specific day by ID.
        /// </summary>
        /// <param name="id">The ID of the company open hours to update.</param>
        /// <param name="openFrom">The new opening time.</param>
        /// <param name="openUntil">The new closing time.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<ServiceResponse> UpdateHoursAsync(string id, TimeSpan openFrom, TimeSpan openUntil)
        {
            try
            {
                // Find the company open hours by ID.
                var item = await _companyOpenHoursRepository.GetFirstAsync(x => x.Id == id);
                if (item is null) return new ServiceResponse(false, "Not Found");

                // Ensure the open from time is earlier than the open until time.
                if (openFrom > openUntil) (openFrom, openUntil) = (openUntil, openFrom);

                // Update the open hours and save changes.
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