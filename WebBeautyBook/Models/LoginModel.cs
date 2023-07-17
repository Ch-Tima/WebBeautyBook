using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "This isn't an email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password address is required.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
