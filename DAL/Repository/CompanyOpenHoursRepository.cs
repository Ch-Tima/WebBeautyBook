using DAL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    /// <summary>
    /// Repository for working with <see cref="CompanyOpenHours"/> entities.
    /// </summary>
    public class CompanyOpenHoursRepository : BaseRepository<CompanyOpenHours, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyOpenHoursRepository"/> class with a database context.
        /// </summary>
        /// <param name="db">The database context to use for data access.</param>
        public CompanyOpenHoursRepository(BeautyBookDbContext db) : base(db)
        {

        }

        /// <summary>
        /// Retrieves the company's schedule with exceptions asynchronously.
        /// </summary>
        /// <param name="companyId">The identifier of the company.</param>
        /// <returns>A collection of CompanyOpenHours objects representing the company's schedule with exceptions.</returns>
        public async Task<IEnumerable<CompanyOpenHours>> FindWithExeceptionAsync(string companyId)
        {
            // Retrieve the list of company open hours for the given company ID
            var companyOpenHoursList = (await GetAllFindAsync(x => x.CompanyId == companyId)).ToList();
            // Retrieve the list of company schedule exceptions for the given company ID
            var companyScheduleExceptionList = await _db.CompanyScheduleExceptions.Where(x => x.CompanyId == companyId).ToListAsync();
            // Get the date of the beginning of the current week
            var dn = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            // Initialize a list to store exceptions for the current week
            var exceptionThisOfWeek = new List<CompanyScheduleException>();

            // If there are no schedule exceptions, return the list of company open hours
            if (companyScheduleExceptionList.Count == 0) return companyOpenHoursList;

            for (byte i = (byte)DayOfWeek.Sunday; i <= (byte)DayOfWeek.Saturday; i++)
            {
                // Find exceptions for the current day
                var item = companyScheduleExceptionList.Find(x => x.ExceptionDate.Day == dn.Day && x.ExceptionDate.Month == dn.Month && x.ExceptionDate.Year <= dn.Year);
                // Add the exception to the list if found
                if (item != null) exceptionThisOfWeek.Add(item);
                // Move to the next day
                dn = dn.AddDays(1);
            }

            // Editing a graph taking into account exceptions
            exceptionThisOfWeek.ForEach(ex =>
            {
                ex.ExceptionDate = ex.ExceptionDate.AddYears(DateTime.Now.Year - ex.ExceptionDate.Year);

                var item = companyOpenHoursList.Find(i => i.DayOfWeek == ((byte)ex.ExceptionDate.DayOfWeek));
                if (item == null && !ex.IsClosed)
                {
                    companyOpenHoursList.Add(new CompanyOpenHours()
                    {
                        Id = "*",
                        DayOfWeek = ((byte)ex.ExceptionDate.DayOfWeek),
                        OpenFrom = ex.OpenFrom,
                        OpenUntil = ex.OpenUntil,
                        CompanyId = ex.CompanyId
                    });
                }
                if (item != null)
                {

                    if (ex.IsClosed)
                    {
                        companyOpenHoursList.Remove(item);
                    }
                    else
                    {
                        item.Id = ex.Reason;
                        item.OpenFrom = ex.OpenFrom;
                        item.OpenUntil = ex.OpenUntil;
                    }
                }

            });

            return companyOpenHoursList;
        }

    }
}
