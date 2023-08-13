using DAL.Repository;
using Domain.Models;

namespace BLL.Services
{
    public class CompanyLikeService
    {
        private readonly CompanyLikeRepository _companyLike;
        private readonly CompanyRepository _company;

        public CompanyLikeService(CompanyLikeRepository companyLike, CompanyRepository company)
        {
            _companyLike = companyLike;
            _company = company;
        }

        public async Task<IEnumerable<CompanyLike>> GetMineAsync(string userId)
        {
            return await _companyLike.GetAllFindIncludeAsync(x => x.UserId == userId);
        }

        public async Task<ServiceResponse> AddAsync(CompanyLike companyLike)
        {
            try
            {
                if (companyLike.CompanyId == null || companyLike.UserId == null)
                    return new ServiceResponse(false, "Fields CompanyId and UserIdb cannot be null");

                var company = await _company.GetAsync(companyLike.CompanyId);
                if (company == null)
                    return new ServiceResponse(false, "Not found compny");

                var duplicate = await _companyLike.GetFirstAsync(x => x.CompanyId == companyLike.CompanyId && x.UserId == companyLike.UserId);
                if (duplicate != null)
                    return new ServiceResponse(false, "You have already added this company to your favorites");

                await _companyLike.InsertAsync(companyLike);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task<ServiceResponse> DeleteAsync(string companyId, string userId)
        {
            try
            {
                var item = await _companyLike.GetFirstAsync(x => x.UserId == userId && x.CompanyId == companyId);

                if (item == null)
                    return new ServiceResponse(false, "Not found");

                await _companyLike.DeleteAsync(item.Id);

                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

    }
}
