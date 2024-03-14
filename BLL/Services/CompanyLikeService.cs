using BLL.Response;
using DAL.Repository;
using Domain.Models;

namespace BLL.Services
{
    /// <summary>
    /// Service class for managing user likes on companies.
    /// </summary>
    public class CompanyLikeService : ServiceBase
    {
        private readonly CompanyLikeRepository _companyLike;
        private readonly CompanyRepository _company;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyLikeService"/> class.
        /// </summary>
        /// <param name="companyLike">The repository for company likes.</param>
        /// <param name="company">The repository for companies.</param>
        public CompanyLikeService(CompanyLikeRepository companyLike, CompanyRepository company)
        {
            _companyLike = companyLike;
            _company = company;
        }

        /// <summary>
        /// Get all companies liked by a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of liked companies.</returns>
        public async Task<IEnumerable<CompanyLike>> GetMineAsync(string userId) => await _companyLike.GetAllFindIncludeAsync(x => x.UserId == userId);

        /// <summary>
        /// Add a company to a user's liked list.
        /// </summary>
        /// <param name="companyLike">The company like to add.</param>
        /// <returns>A <see cref="IServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<IServiceResponse> AddAsync(CompanyLike companyLike)
        {
            try
            {
                if (companyLike.CompanyId == null || companyLike.UserId == null)
                    return BadResult("Fields CompanyId and UserIdb cannot be null");

                var company = await _company.GetAsync(companyLike.CompanyId);
                if (company == null)
                    return BadResult("Not found compny");

                var duplicate = await _companyLike.GetFirstAsync(x => x.CompanyId == companyLike.CompanyId && x.UserId == companyLike.UserId);
                if (duplicate != null)
                    return BadResult("You have already added this company to your favorites");

                await _companyLike.InsertAsync(companyLike);

                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

        /// <summary>
        /// Delete a company from a user's liked list.
        /// </summary>
        /// <param name="companyId">The ID of the company to delete.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A <see cref="IServiceResponse"/> indicating the result of the operation.</returns>
        public async Task<IServiceResponse> DeleteAsync(string companyId, string userId)
        {
            try
            {
                var item = await _companyLike.GetFirstAsync(x => x.UserId == userId && x.CompanyId == companyId);
                if (item is null)
                    return BadResult("Not found");

                await _companyLike.DeleteAsync(item.Id);
                return OkResult();
            }
            catch (Exception ex)
            {
                return BadResult(ex.Message);
            }
        }

    }
}
