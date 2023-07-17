using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "This isn't an email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName is required!")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "The \"Name\" must be between 4 and 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "UserSurname is required!")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "The \"Surname\" must be between 4 and 100 characters.")]
        public string UserSurname { get; set; }


        [Required(ErrorMessage = "Password is required!")]
        [MinLength(6, ErrorMessage = "The minimum Password length is 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(6, ErrorMessage = "The minimum Password length is 6 characters.")]
        [Compare("Password", ErrorMessage = "ConfirmPassword and Password do not match.")]
        public string ConfirmPassword { get; set; }


        /// <summary>
        /// Default role: <see cref="Roles.CLIENT"><c>Roles.CLIENT</c></see>
        /// </summary>
        public string Role { get; set; } = Roles.CLIENT;
    }
}
