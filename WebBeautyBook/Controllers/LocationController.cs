using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("getAll")]
        public async Task<IEnumerable<Location>> GetAll()
        {
            return await _locationService.GetAllAsync();
        }

        [HttpGet("getAllCountry")]
        public async Task<IEnumerable<Location>> getAllCountry()
        {
            return await _locationService.GetUniqueCountry();
        }

        [HttpGet("getAllCity")]
        public async Task<IEnumerable<Location>> getAllCity(string contry)
        {
            return await _locationService.GetAllFindAsync(l => l.Country == contry);
        }

        [HttpPut]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> Insert([FromBody] LocationViewModel model)
        {
            var location = new Location()
            {
                City = model.City,
                Country = model.Country
            };

            var result = await _locationService.InsertAsync(location);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return new OkObjectResult(location);
        }

        [HttpDelete]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> Delete()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> Update([FromBody] LocationViewModel model, string Id)
        {
            var location = new Location()
            {
                Id = Id,
                City = model.City,
                Country = model.Country
            };

            var result = await _locationService.UpdataAsync(location);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();
        }

    }
}
