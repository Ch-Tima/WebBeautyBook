using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email token is required.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "This isn't an email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(6, ErrorMessage = "The minimum Password length is 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(6, ErrorMessage = "The minimum Password length is 6 characters.")]
        [Compare("Password", ErrorMessage = "ConfirmPassword and Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
