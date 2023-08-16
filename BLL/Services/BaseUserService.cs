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

        public async Task<ServiceResponse> UpdateAsync(BaseUser baseUser)
        {
            try
            {
                if(baseUser.PhoneNumber != null)
                {
                    var duplicatePhoneNumber = await _baseUserRepository.GetFirstAsync(x => x.PhoneNumber == baseUser.PhoneNumber && x.Id != baseUser.Id);
                    if(duplicatePhoneNumber != null)
                        return new ServiceResponse(false, "This phone number is already in use.");
                }

                await _baseUserRepository.UpdateAsync(baseUser.Id, baseUser);
                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

    }
}
