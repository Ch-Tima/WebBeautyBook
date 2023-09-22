using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        private readonly LocationService _locationService;
        private readonly IConfiguration _configuration;
        
        public LocationController(LocationService locationService, IConfiguration configuration)
        {
            _locationService = locationService;
            _configuration = configuration;
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

        /// <summary>
        /// Retrieves the user's location based on their IP address.
        /// </summary>
        /// <remarks>
        /// This method uses the user's IP address to determine their approximate location by making a request to an external IP information API.
        /// If the IP address is null, it returns a BadRequest response.
        /// </remarks>
        /// <returns>
        /// An IActionResult representing the user's location information, including their country, city, and coordinates.
        /// If an error occurs during the process, a BadRequest response with an error message is returned.
        /// </returns>
        [HttpGet(nameof(GetUserLocationViaIP))]
        public async Task<IActionResult> GetUserLocationViaIP()
        {
            var token = _configuration.GetValue<string>("IpInfoToken");
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ip is null) return BadRequest("RemoteIpAddress is null");
            using (var httpClient = new HttpClient())// Create an HttpClient to make a request to the IP information API
            {
                try
                {
                    // Make an asynchronous GET request to the IP information API
                    var response = await httpClient.GetStringAsync($"https://ipinfo.io/{ip}?token={token}");
                    // Deserialize the response JSON into a LocationIPAPI object
                    var locationIpApi = JsonConvert.DeserializeObject<LocationIPAPI>(response);
                    return Ok(locationIpApi);
                }
                catch (HttpRequestException e)
                {
                    // Handle any errors that occur during the HTTP request and return a BadRequest response with the error message.
                    return BadRequest(e.Message);
                }
            }
        }
        
    }
}
