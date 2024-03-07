using DAL.Repository;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CompanyScheduleExceptionService
    {
        private readonly CompanyScheduleExceptionRepository _scheduleExceptionRepository;
        private readonly CompanyRepository _companyRepository;

        public CompanyScheduleExceptionService(CompanyScheduleExceptionRepository scheduleExceptionRepository, CompanyRepository companyRepository)
        {
            _scheduleExceptionRepository = scheduleExceptionRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<CompanyScheduleException>> FindAsync(string compoanyId)
        {
            var t = await _scheduleExceptionRepository.GetAllFindAsync(x => x.CompanyId == compoanyId);
            return t;
        }

        public async Task<ServiceResponse> AddAsync(CompanyScheduleException model)
        {
            try
            {

                var company = await _companyRepository.GetAsync(model.CompanyId);
                if (company == null)
                    return new ServiceResponse(false, "Company not found");

                await _scheduleExceptionRepository.InsertAsync(model);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task<ServiceResponse> DeleteAsynce(string id, string  companyId)
        {

            try
            {
                var sE = await _scheduleExceptionRepository.GetAsync(id);

                if (sE == null)
                    return new ServiceResponse(false, "Not found ");

                if (sE.CompanyId != companyId)
                    return new ServiceResponse(false, "Sorry, you can't do that.");

                await _scheduleExceptionRepository.DeleteAsync(id);

                return new ServiceResponse(true, "Ok");
            
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }

        }


    }
}
