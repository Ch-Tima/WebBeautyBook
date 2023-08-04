using DAL.Repository;
using Domain.Models;

namespace BLL.Services
{
    public class CompanyOpenHoursService
    {
        private readonly CompanyOpenHoursRepository _companyOpenHoursRepository;
        private readonly CompanyRepository _companyRepository;
        public CompanyOpenHoursService(CompanyOpenHoursRepository companyOpenHoursRepository, CompanyRepository companyRepository)
        {
            _companyOpenHoursRepository = companyOpenHoursRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<CompanyOpenHours>> FindAsync(string companyId)
        {
            var result = await _companyOpenHoursRepository.GetAllFindAsync(x => x.CompanyId == companyId);
            return result;
        }

        public async Task<ServiceResponse> AddAsync(CompanyOpenHours companyOpenHours, string companyId)
        {
            try
            {

                if (companyOpenHours.DayOfWeek < 0 || companyOpenHours.DayOfWeek > 7)
                    return new ServiceResponse(false, "There is no such day");

                if (companyId == null)
                    return new ServiceResponse(false, "Company ID cannot be null");

                var company = await _companyRepository.GetAsync(companyId);
                if (company == null)
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

    }
}
