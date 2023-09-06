using DAL.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class LocationService
    {
        private readonly LocationRepository _locationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class.
        /// </summary>
        /// <param name="locationRepository">The repository for locations.</param>
        public LocationService(LocationRepository locationRepository) 
        {
            _locationRepository = locationRepository;
        }

        public async Task<Location> GetAsync(string id)
        {
            return await _locationRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Location>> GetUniqueCountry()
        {
            var t = await _locationRepository.GetAllAsync();
            return t.DistinctBy(x => x.Country);
        }

        public async Task<IEnumerable<Location>> GetAllFindAsync(Expression<Func<Location, bool>> func)
        {
            return await _locationRepository.GetAllFindAsync(func);
        }

        /// <summary>
        /// Inserts a new location asynchronously.
        /// </summary>
        /// <param name="location">The location to insert.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the insertion.</returns>
        public async Task<ServiceResponse> InsertAsync(Location location)
        {
            try
            {
                //Check for duplicate Location
                var findDuplicate = await _locationRepository.GetFirstAsync(x => x.City.ToUpper() == location.City.ToUpper() && x.Country.ToUpper() == location.Country.ToUpper());
                if (findDuplicate != null) return new ServiceResponse(false, $"Location with country: {location.Country.ToUpper()} and city: {location.City.ToUpper()} exists.");

                await _locationRepository.InsertAsync(location);
                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }

        public async Task DeleteAsync(string id)
        {
            await _locationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Updates an existing location asynchronously.
        /// </summary>
        /// <param name="newLocation">The updated location.</param>
        /// <returns>A <see cref="ServiceResponse"/> indicating the result of the update.</returns>
        public async Task<ServiceResponse> UpdataAsync(Location newLocation)
        {
            try
            {
                //Check for duplicate Location
                var findDuplicate = await _locationRepository.GetFirstAsync(x => x.City.ToUpper() == newLocation.City.ToUpper() && x.Country.ToUpper() == newLocation.Country.ToUpper());
                if (findDuplicate != null) return new ServiceResponse(false, $"Location with country: {newLocation.Country.ToUpper()} and city: {newLocation.City.ToUpper()} exists.");

                await _locationRepository.UpdateAsync(newLocation.Id, newLocation);
                return new ServiceResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }

        }

    }
}
