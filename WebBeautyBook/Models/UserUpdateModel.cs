using System.ComponentModel.DataAnnotations;
using WebBeautyBook.Validations;

namespace WebBeautyBook.Models
{
    public class UserUpdateModel
    {
        [Required(ErrorMessage = "Name is required!w")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "The \"Name\" must be between 4 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required!")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "The \"Surname\" must be between 4 and 100 characters.")]
        public string Surname { get; set; }

        [RegularExpression(@"(^$)|([+0-9]{12})", ErrorMessage = "This is not a phone number.")]
        public string? PhoneNumber { get; set; }

        [MaxFileSize(8 * 1024 * 1024, ErrorMessage = "Maximum photo size 8Mb")]
        [ExtensionsIFormFile(fileExtensions: "png,jpg,jpeg", ErrorMessage = "It's not invalid format.")]
        public IFormFile? File { get; set; }

    }
}
