using BLL.Providers;
using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq.Expressions;
using System.Text;
using WebBeautyBook.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        private readonly UserManager<BaseUser> _userManager;
        private readonly CompanyService _companyService;
        private readonly WorkerService _workerService;
        private readonly IEmailSender _emailService;

        public CompanyController(UserManager<BaseUser> userManager, CompanyService companyService, 
            WorkerService workerService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _companyService = companyService;
            _workerService = workerService;
            _emailService = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            if (id == null) return BadRequest("Id cannot null");

            var company = await _companyService.GetIncludeForClientAsync(id);

            if (company == null) return BadRequest("Comapny not found");

            return Ok(company);
        }

        [HttpGet("getAll")]
        public async Task<IEnumerable<Company>> GetAll() => await _companyService.GetAllAsync();

        /// <summary>
        /// ! TODO !
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpGet("getTopTen")]
        public async Task<IEnumerable<Company>> GetTopTen(string? location)
        {
            return await _companyService.GetTopTen(location);
        }

        /// <summary>
        /// Searches for companies based on name, category, and location criteria.
        /// </summary>
        /// <param name="name">The name to filter companies by.</param>
        /// <param name="category">The category to filter companies by.</param>
        /// <param name="location">The location to filter companies by.</param>
        /// <returns>A collection of companies that match the specified criteria.</returns>
        [HttpGet(nameof(Search))]
        public async Task<IEnumerable<Company>> Search(string? name, string? category, string? location)
        {
            //The filter expression based on the provided parameters
            Expression<Func<Company, bool>> filterExpression = company => 
                (string.IsNullOrEmpty(name) || company.Name.Contains(name)) && 
                (string.IsNullOrEmpty(category) || company.Services.Any(s => s.Category.Name.Contains(category))) &&
                (string.IsNullOrEmpty(location) || (company.Location.Country + " " + company.Location.City + " " + company.Address).Contains(location));
            return await _companyService.GetFindIncludeAsync(filterExpression);;
        }

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

        [HttpPost(template: "invitationToCompany")]
        [Authorize(Roles = Roles.OWN_COMPANY)]
        public async Task<IActionResult> InvitationToCompany([FromBody]string email)
        {
            var own = await _userManager.GetUserAsync(User);
            var workerOwn = await _workerService.GetIncudeAsync(own.WorkerId);

            if (workerOwn == null)
                return BadRequest("Most likely, you do not belong to any company.");

            var futureWorker = await _userManager.FindByEmailAsync(email);

            if (futureWorker == null)
                return BadRequest($"User not found with email: {email}.");

            if (futureWorker.WorkerId != null)
                return BadRequest("This user belongs to another company.");



            //create token
            var token = await _userManager.GenerateUserTokenAsync(user: futureWorker, tokenProvider: InvitationToCompanyTokenProviderOptions.TokenProvider, purpose: $"JoiningTheCompany{workerOwn.CompanyId}");

            var param = new Dictionary<string, string>
            {
                {"token", WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)) },
                {"companyId", WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(workerOwn.CompanyId)) }
            };
            var invitationToCompany = QueryHelpers.AddQueryString(Request.Scheme + "://" + Request.Host.Value + "/acceptInvitation", param);
            
            //Send email
            var msgHtml = $"<label>Dear {futureWorker.UserName} {futureWorker.UserSurname}!</label><br>" +
                $"<p>You are invited by the company {workerOwn.Company.Name} to become their employee:</p>" +
                $"<em>&emsp;<a href='{invitationToCompany}'>Accept invitation</a></em>" +
                $"<p>If you click on \"Accept invitation\" you will be automatically added to {workerOwn.Company.Name} as a worker! If you think this message is an error, you can add it to spam.</p>";

            await _emailService.SendEmailAsync(futureWorker.Email, "Invitation to company", msgHtml);

            //($"We sent an invitation to the company for the user {futureWorker.UserName}.");

            return new JsonResult($"We sent an invitation to the company for the user {futureWorker.UserName}.")
            {
                StatusCode = 200,
            };
        }

        /// <summary>
        /// Adds the invited user to the company and creates a <see cref="Worker"/> profile.
        /// </summary>
        /// <param name="token">The company invitation is encrypted with <see cref="WebEncoders.Base64UrlEncode"></see> with UTF-8 format</param>
        /// <param name="companyId">Encrypted in the same way as the <paramref name="token"/>, needed to check the <paramref name="token"/> for <c>"purpose"</c> and add the user to the company</param>
        /// <returns></returns>
        [HttpPost(template: "acceptInvitationToCompany")]
        [Authorize]
        public async Task<IActionResult> AcceptInvitationToCompany(string token, string companyId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user.WorkerId != null)//check if the user belongs to some company
                    return BadRequest("You are already in the company.");

                //decode companyId
                companyId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(companyId));
                var company = await _companyService.GetAsync(companyId);//find company
                if(company == null)
                    return BadRequest("This companyId is invalid.");

                //verify token
                var result = await _userManager.VerifyUserTokenAsync(
                    user: user, 
                    tokenProvider: InvitationToCompanyTokenProviderOptions.TokenProvider,
                    purpose: $"JoiningTheCompany{companyId}",
                    token: Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)));

                if (!result)
                    return BadRequest("This token is invalid.");

                //add role for user "WORKER"
                var roleResult = await _userManager.AddToRoleAsync(user, Roles.WORKER);
                if (!roleResult.Succeeded)
                    return BadRequest("Role assigment error.");

                //add user to company
                var resultWorker = await _workerService.InsertAsync(companyId, user);
                //if it was not possible to add a user to the company, then delete the role "worker"
                if (!resultWorker.IsSuccess)
                {
                    await _userManager.RemoveFromRoleAsync(user, Roles.WORKER);
                    return BadRequest(resultWorker.Message);
                }

                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
