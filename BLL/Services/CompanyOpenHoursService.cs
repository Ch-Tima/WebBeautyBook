﻿using BLL.Response;
using DAL.Repository;
using Domain.Models;

namespace BLL.Services
{
    /// <summary>
    /// Service for managing company open hours.
    /// </summary>
    public class CompanyOpenHoursService : ServiceBase
    {
        private readonly CompanyOpenHoursRepository _companyOpenHoursRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly CompanyScheduleExceptionRepository _scheduleExceptionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyOpenHoursService"/> class.
        /// </summary>
        /// <param name="companyOpenHoursRepository">The repository for company open hours.</param>
        /// <param name="companyRepository">The repository for companies.</param>
        public CompanyOpenHoursService(CompanyOpenHoursRepository companyOpenHoursRepository, CompanyRepository companyRepository, CompanyScheduleExceptionRepository scheduleExceptionRepository)
        {
            _companyOpenHoursRepository = companyOpenHoursRepository;
            _companyRepository = companyRepository;
            _scheduleExceptionRepository = scheduleExceptionRepository;
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
        /// Retrieves the company's schedule with exceptions asynchronously.
        /// </summary>
        /// <param name="companyId">The identifier of the company.</param>
        /// <returns>A collection of CompanyOpenHours objects representing the company's schedule.</returns>
        public async Task<IEnumerable<CompanyOpenHours>> FindWithExeceptionAsync(string companyId)
        {
            return await _companyOpenHoursRepository.FindWithExeceptionAsync(companyId);           
        }


        /// <summary>
        /// Add company open hours for a specific day of the week.
        /// </summary>
        /// <param name="companyOpenHours">The open hours to add.</param>
        /// <param name="companyId">The ID of the company to add open hours for.</param>
        /// <returns>A <see cref="IServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<IServiceResponse> AddAsync(CompanyOpenHours companyOpenHours)
        {
            try
            {
                // Check if the day of the week is valid.
                if (companyOpenHours.DayOfWeek < 0 || companyOpenHours.DayOfWeek > 7)
                    return BadResult("There is no such day");

                // Check if the company ID is null.
                if (companyOpenHours.CompanyId is null)
                    return BadResult("Company ID cannot be null");

                // Retrieve the company by its ID.
                var company = await _companyRepository.GetAsync(companyOpenHours.CompanyId);
                if (company is null)
                    return BadResult("Not found company");

                // Check for duplicate open hours for the same day and company.
                var findDuplicate = await _companyOpenHoursRepository.GetFirstAsync(x => x.DayOfWeek == companyOpenHours.DayOfWeek && x.CompanyId == companyOpenHours.CompanyId);
                if (findDuplicate is not null)
                    return BadResult("There is a schedule for the day");

                // Ensure the open from time is earlier than the open until time.
                if (companyOpenHours.OpenFrom > companyOpenHours.OpenUntil) 
                    (companyOpenHours.OpenFrom, companyOpenHours.OpenUntil) = (companyOpenHours.OpenUntil, companyOpenHours.OpenFrom);

                // Insert the new open hours into the repository.
                await _companyOpenHoursRepository.InsertAsync(companyOpenHours);

                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

        /// <summary>
        /// Delete company open hours by ID.
        /// </summary>
        /// <param name="id">The ID of the company open hours to delete.</param>
        /// <returns>A <see cref="IServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<IServiceResponse> DeleteAsynce(string id)
        {
            try
            {
                // Find the company open hours by ID.
                var item = await _companyOpenHoursRepository.GetFirstAsync(x => x.Id ==  id);

                // If the item is not found, return a corresponding error response.
                if (item == null) return BadResult("Not found");

                // Delete the company open hours from the repository.
                await _companyOpenHoursRepository.DeleteAsync(id);

                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

        /// <summary>
        /// Update company open hours for a specific day by ID.
        /// </summary>
        /// <param name="id">The ID of the company open hours to update.</param>
        /// <param name="openFrom">The new opening time.</param>
        /// <param name="openUntil">The new closing time.</param>
        /// <returns>A <see cref="IServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<IServiceResponse> UpdateHoursAsync(string id, TimeSpan openFrom, TimeSpan openUntil)
        {
            try
            {
                // Find the company open hours by ID.
                var item = await _companyOpenHoursRepository.GetFirstAsync(x => x.Id == id);
                if (item is null) return BadResult("Not Found");

                // Ensure the open from time is earlier than the open until time.
                if (openFrom > openUntil) (openFrom, openUntil) = (openUntil, openFrom);

                // Update the open hours and save changes.
                item.OpenFrom = openFrom;
                item.OpenUntil = openUntil;

                await _companyOpenHoursRepository.UpdateAsync(id, item);
                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

    }
}