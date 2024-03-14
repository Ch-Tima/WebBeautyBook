using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CompanyImageService : ServiceBase
    {
        private readonly CompanyImagesRepository _imagesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyImageService"/> class.
        /// </summary>
        /// <param name="imagesRepository">The repository for company images.</param>
        public CompanyImageService(CompanyImagesRepository imagesRepository) 
        {
            _imagesRepository = imagesRepository;
        }

        /// <summary>
        /// Get a company image by its ID.
        /// </summary>
        /// <param name="id">The ID of the company image to retrieve.</param>
        /// <returns>The requested company image.</returns>
        public async Task<CompanyImage> Get(string id) => await _imagesRepository.GetAsync(id);

        /// <summary>
        /// Get all company images.
        /// </summary>
        /// <returns>A collection of all company images.</returns>
        public async Task<IEnumerable<CompanyImage>> GetAll() => await _imagesRepository.GetAllAsync();

        /// <summary>
        /// Get company images that match the specified filter expression.
        /// </summary>
        /// <param name="expression">The filter expression to match company images.</param>
        /// <returns>A collection of matching company images.</returns>
        public async Task<IEnumerable<CompanyImage>> GetFindAsync(Expression<Func<CompanyImage, bool>> expression) => await _imagesRepository.GetAllFindAsync(expression);

        /// <summary>
        /// Insert a new company image into the repository.
        /// </summary>
        /// <param name="companyImage">The company image to insert.</param>
        public async Task InsertAsync(CompanyImage companyImage)
        {
            await _imagesRepository.InsertAsync(companyImage);
        }

        /// <summary>
        /// Delete a company image by its ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the company image to delete.</param>
        public async Task DeleteAsync(string id)
        {
            await _imagesRepository.DeleteAsync(id);
        }
    }
}
