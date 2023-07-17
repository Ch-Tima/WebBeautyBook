using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBeautyBook.Models;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        private readonly UserManager<BaseUser> _userManager;
        private readonly CompanyService _companyService;
        private readonly BLL.Services.WorkerService _workerService;

        public CompanyController(UserManager<BaseUser> userManager, CompanyService companyService,
            BLL.Services.WorkerService workerService)
        {
            _userManager = userManager;
            _companyService = companyService;
            _workerService = workerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (id == null) return BadRequest("Id cannot null");

            var company = await _companyService.GetIncludeAsync(id);

            if (company == null) return BadRequest("Comapny not found");


            return Ok(company);
        }

        [HttpGet("getAll")]
        public async Task<IEnumerable<Company>> GetAll() => await _companyService.GetAllAsync();

        [HttpGet("getMyCompany")]
        [Authorize(Roles = Roles.OWN_COMPANY + "," + Roles.WORKER)]
        public async Task<IActionResult> GetMyCompany()
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null) return BadRequest("Sorry, we can't find your user data.");

            var company = await _companyService.GetAllFindAsync(x => x.Workers.Any(w => w.BaseUserId == user.Id));

            if (company == null || company.Count() <= 0) return BadRequest("Most likely, you do not belong to any company.");

            return Ok(company.First());
        }

        [HttpGet("getWorkers")]
        [Authorize(Roles = Roles.OWN_COMPANY + "," + Roles.WORKER)]
        public async Task<IActionResult> GetWorkers()
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return BadRequest("Sorry, we can't find your user data.");

            var worker = await _workerService.GetAsync(user.WorkerId);

            if (worker == null)
                return BadRequest("Most likely, you do not belong to any company.");

            var workers = await _workerService.GetAllIncludeFindAsync(x => x.CompanyId == worker.CompanyId);

            return Ok(workers);
        }

        [HttpPut]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> InserCompany(RegistrationCompanyModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailOwn);

            if (user == null)//Does the user exist
                return BadRequest($"User not found with email: {model.EmailOwn}.");

            //Does the user belong to a company
            if (user.WorkerId != null && (await _workerService.GetAsync(user.WorkerId)) != null)
                return BadRequest($"User {model.EmailOwn} has a company.");

            //If role user is not OWN_COMPANY
            if (!await _userManager.IsInRoleAsync(user, Roles.OWN_COMPANY))
                return BadRequest($"The user does not have the {Roles.OWN_COMPANY} role.");

            //create Company
            var company = new Company()
            {
                Name = model.NameCompany,
                Email = model.FeedbackEmail,
                Address = model.Address,
                LocationId = model.LocationId,
            };
            await _companyService.CreateCompany(company, user);

            //Clear navigation fields
            company.Workers = new List<Worker>();
            company.Services = new List<Service>();

            return new OkObjectResult(company);
        }

        [HttpPost]
        [Authorize(Roles = Roles.OWN_COMPANY)]
        public async Task<IActionResult> InsertWorkerToCompany(string email)
        {
            var own = await _userManager.GetUserAsync(User);
            var workerOwn = await _workerService.GetAsync(own.WorkerId);

            if (workerOwn == null)
                return BadRequest("Most likely, you do not belong to any company.");

            var futureWorker = await _userManager.FindByEmailAsync(email);

            if (futureWorker == null)
                return BadRequest($"User not found with email: {email}.");

            if (!await _userManager.IsInRoleAsync(futureWorker, Roles.WORKER))//Does the user have permission
                return BadRequest($"This user does not have {Roles.WORKER} permission.");

            //TODO send invitation in company
            var result = await _workerService.InsertAsync(workerOwn.CompanyId, futureWorker);

            return result.IsSuccess ? Ok() : BadRequest(result.Message);
        }

    }
}
