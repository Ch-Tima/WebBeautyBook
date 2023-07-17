using DAL.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class LocationService
    {
        private readonly LocationRepository _locationRepository;

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

        public async Task InsertAsync(Location location)
        {
            await _locationRepository.InsertAsync(location);
        }

        public async Task DeleteAsync(string id)
        {
            await _locationRepository.DeleteAsync(id);
        }

        public async Task UpdataAsync(Location newLocation)
        {
            if (newLocation == null) return;

            await _locationRepository.UpdateAsync(newLocation.Id, newLocation);
        }

    }
}
