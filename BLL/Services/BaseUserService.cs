

using DAL.Repository;
using Domain.Models;

namespace BLL.Services
{
    public class BaseUserService
    {

        private readonly BaseUserRepository _baseUserRepository;

        public BaseUserService(BaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }

        public async Task<BaseUser?> GetAsync(string id)
        {
            if (id == null) return null;

            return await _baseUserRepository.GetAsync(id);
        }

        public async Task UpdateAsync(BaseUser baseUser)
        {
           await _baseUserRepository.UpdateAsync(baseUser.Id, baseUser);
        }

    }
}
