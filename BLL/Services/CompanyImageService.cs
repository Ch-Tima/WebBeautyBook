using DAL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CompanyImageService
    {
        private readonly CompanyImagesRepository _imagesRepository;
        public CompanyImageService(CompanyImagesRepository imagesRepository) 
        {
            _imagesRepository = imagesRepository;
        }

        public async Task<CompanyImage> Get(string id)
        {
            return await _imagesRepository.GetAsync(id);
        }

        public async Task<IEnumerable<CompanyImage>> GetAll()
        {
            return await _imagesRepository.GetAllAsync();
        }
        public async Task<IEnumerable<CompanyImage>> GetFindAsync(Expression<Func<CompanyImage, bool>> expression)
        {
            return await _imagesRepository.GetAllFindAsync(expression);
        }

        public async Task InsertAsync(CompanyImage companyImage)
        {
            await _imagesRepository.InsertAsync(companyImage);
        }

        public async Task DeleteAsync(string id)
        {
            await _imagesRepository.DeleteAsync(id);
        }
    }
}
