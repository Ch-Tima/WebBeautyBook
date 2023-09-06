﻿using DAL.Repository;
using Domain.Models;

namespace BLL.Services
{
    public class BaseUserService
    {

        private readonly BaseUserRepository _baseUserRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserService"/> class.
        /// </summary>
        /// <param name="baseUserRepository">The repository for base user data.</param>
        public BaseUserService(BaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }

        /// <summary>
        /// Get a base user by their ID.
        /// </summary>
        /// <param name="id">The ID of the base user to retrieve.</param>
        /// <returns>The requested base user, or null if the ID is null.</returns>
        public async Task<BaseUser?> GetAsync(string id)
        {
            if (id is null) return null;
            return await _baseUserRepository.GetAsync(id);
        }

        /// <summary>
        /// Update a base user's information.
        /// </summary>
        /// <param name="baseUser">The updated base user information.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the update operation.</returns>
        public async Task<ServiceResponse> UpdateAsync(BaseUser baseUser)
        {
            try
            {
                if(baseUser.PhoneNumber != null)//Check for duplicate phone numbers, but only if a phone number is provided.
                {
                    var duplicatePhoneNumber = await _baseUserRepository.GetFirstAsync(x => x.PhoneNumber == baseUser.PhoneNumber && x.Id != baseUser.Id);
                    if(duplicatePhoneNumber != null)
                        return new ServiceResponse(false, "This phone number is already in use.");
                }
                //Update the base user information in the repository.
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
