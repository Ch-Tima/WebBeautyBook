﻿using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebBeautyBook.Models;
using BLL.Services;
using Microsoft.Extensions.Localization;

namespace WebBeautyBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<BaseUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly WorkerService _workerService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailService;

        private readonly IStringLocalizer<AuthController> _localizer;

        public AuthController(UserManager<BaseUser> userManager, RoleManager<IdentityRole> roleManager, IStringLocalizer<AuthController> localizer,
            WorkerService workerService, IConfiguration configuration, IEmailSender emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _workerService = workerService;
            _configuration = configuration;
            _emailService = emailService;
            _localizer = localizer;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new BadRequestObjectResult(_localizer["IncorrectEmailOrPassword"].Value);

            return new OkObjectResult(await GenerateJWTAsync(user, 7));

        }

        /// <summary>
        /// Refreshes the authentication tokens for the currently authenticated user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the refreshed JWT token details.</returns>
        [HttpPost(template: "refreshTokens")]
        [Authorize]
        public async Task<IActionResult> RefreshTokens()
        {
            var user = await _userManager.GetUserAsync(User);//Retrieve the current user based on the authenticated user's claims.
            return new OkObjectResult(await GenerateJWTAsync(user, 7));//Generate and return a new JWT token with an extended validity period (e.g., 7 days).
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            model.Role = Roles.CLIENT;
            return await BaseRegister(model);
        }

        [HttpPost]
        [Route("registrationViaAdmin")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> RegistrationViaAdmin([FromBody] RegistrationModel model)
        {
            return await BaseRegister(model);
        }

        [HttpPost]
        [Route("registrationViaCompany")]
        [Authorize(Roles = Roles.OWN_COMPANY)]
        public async Task<IActionResult> RegistrationViaCompany([FromBody] RegistrationModel model)
        {
            if (model.Role == Roles.CLIENT) return BadRequest(_localizer["RoleIsInvalid", model.Role].Value);

            var own = await _userManager.GetUserAsync(User);
            var ownWork = await _workerService.GetAsync(own.WorkerId);
            if (own == null || ownWork == null) return BadRequest(_localizer["YouDontHaveCompany"].Value);

            var result = await BaseRegister(model);
            var code = ((IStatusCodeActionResult)result).StatusCode;

            if (code != 200) return result;

            //Insert new Worker to company
            var newWorker = await _userManager.FindByEmailAsync(model.Email);
            await _workerService.InsertAsync(ownWork.CompanyId, newWorker);

            newWorker.Worker = null;

            return result;
        }

        /// <summary>
        /// Handles the HTTP POST request for initiating the password reset process by sending a reset link to the user's email.
        /// </summary>
        /// <param name="email">The email address of the user requesting a password reset.</param>
        /// <returns>An IActionResult indicating the result of the password reset initiation.</returns>
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new BadRequestObjectResult(_localizer["UserWithNameCouldntBeFound", email].Value);

            if (!user.EmailConfirmed)
            {
                //Send email with user email confirmation
                var emailConfirmLink = await GenerateEmailConfirmationLinkAsync(user);
                var msgHtml = _localizer["PleaseConfirmEmail", emailConfirmLink].Value;
                await _emailService.SendEmailAsync(user.Email, "Confirmation Email(WebBeautyBook)", msgHtml);
                return new BadRequestObjectResult(_localizer["PleaseVerifyEmail", email].Value);
            }

            var link = await GeneratePasswordResetLinkAsync(user);
            await _emailService.SendEmailAsync(email, "Reset Password(WebBeautyBook)", _localizer["MsgResetPassword", link].Value);

            return Ok();
        }

        /// <summary>
        /// Handles the HTTP POST request for resetting a user's password.
        /// </summary>
        /// <param name="model">A model containing email, token, and new password.</param>
        /// <returns>An IActionResult indicating the result of the password reset operation.</returns>
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return BadRequest(_localizer["UserWithNameCouldntBeFound", model.Email]);

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded) return BadRequest(_localizer["TokenIsNotCorrect"].Value);

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IEnumerable<BaseUser>> FindUsers(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }

        /// <summary>
        /// Handles the HTTP GET request for confirming a user's email address using a confirmation token.
        /// </summary>
        /// <param name="token">The confirmation token sent to the user's email.</param>
        /// <param name="email">The email address of the user whose email is being confirmed.</param>
        /// <returns>An IActionResult indicating the result of the email confirmation.</returns>
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return BadRequest(_localizer["UserWithNameCouldntBeFound", email]);
            if (user.EmailConfirmed) return BadRequest(_localizer["EmailIsConfirmation", email].Value);

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded) return BadRequest(_localizer["TokenIsNotCorrect"].Value);

            await _emailService.SendEmailAsync(email, "Confirmation Email(WebBeautyBook)", _localizer["MsgConfirmationEmail"].Value);

            return Ok();
        }

        /// <summary>
        /// Method for handling user registration based on the provided <see cref="RegistrationModel"/>.
        /// </summary>
        /// <param name="model">user data</param>
        /// <returns>Task<see cref="IActionResult">&lt;IActionResult&gt;</see></returns>
        private async Task<IActionResult> BaseRegister(RegistrationModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            //Check if the provided email address is already in use, return a BadRequest if it is.
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return BadRequest(_localizer["EmailIsBusy", model.Email].Value);
            //Check if the specified role exists; return a BadRequest if it doesn't.
            if (!await _roleManager.RoleExistsAsync(model.Role))
                return BadRequest(_localizer["InvalidRole", model.Role]);
            //Create a new user object with the provided information.e
            var user = new BaseUser
            {
                UserName = model.UserName,
                UserSurname = model.UserSurname,
                Email = model.Email,
            };
            var userCreated = await _userManager.CreateAsync(user, model.Password);
            //Check the result of user creation. If it fails, return a BadRequest with error details.
            if (!userCreated.Succeeded)
                return new BadRequestObjectResult(string.Join(", ", userCreated.Errors.Select(error => $"{error.Code} {error.Description}")));
            //Add role for user.
            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
            //Send the confirmation email to the user's email address
            var link = await GenerateEmailConfirmationLinkAsync(user);
            var msgHtml = _localizer["PleaseConfirmEmail", link].Value;
            await _emailService.SendEmailAsync(user.Email, "Confirmation Email(WebBeautyBook)", msgHtml);
            //Return an OkObjectResult with the registered user's details.
            return new OkObjectResult(user);
        }

        /// <summary>
        /// <br>Generates a link with a token and an email to verify the user's email.</br>
        /// </summary>
        /// <param name="user">The user who needs to generate a link to confirm the email</param>
        /// <returns></returns>
        private async Task<string> GenerateEmailConfirmationLinkAsync(BaseUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);//error
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", user.Email }
            };
            var confirmationLink = QueryHelpers.AddQueryString(Request.Scheme + "://" + Request.Host.Value + "/emailConfirmation", param);
            return confirmationLink;
        }

        /// <summary>
        /// <br>Generates a link with a token and an email to reset the user's password.</br>
        /// <br>The link leads to the handicap <c>"/resetPassword"</c>.</br>
        /// </summary>
        /// <param name="user">The user who needs to generate a link to reset the password</param>
        /// <returns></returns>
        private async Task<string> GeneratePasswordResetLinkAsync(BaseUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", user.Email }
            };
            var passwordResetLink = QueryHelpers.AddQueryString(Request.Scheme + "://" + Request.Host.Value + "/resetPassword", param);
            return passwordResetLink;
        }

        /// <summary>
        /// Generates JWT for authorization in the system.
        /// </summary>
        /// <param name="user">The user who needs to generate the "JWT" token.</param>
        /// <param name="days">The number of days of life of the "JWT" token</param>
        /// <returns></returns>
        private async Task<TokenDetails> GenerateJWTAsync(BaseUser user, int days)
        {
            var claims = new List<Claim>
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in await _userManager.GetRolesAsync(user))
                claims.Add(new Claim(ClaimTypes.Role, userRole));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(days),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new TokenDetails(
                Token: new JwtSecurityTokenHandler().WriteToken(token),
                Expiration: token.ValidTo
            );
        }
    }

}
