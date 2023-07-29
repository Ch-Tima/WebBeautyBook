using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class RegistrationCompanyModel
    {

        [Required(ErrorMessage = "NameCompany is required!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The NameCompany must be between 3 and 100 characters.")]
        public string NameCompany { get; set; }

        [Required(ErrorMessage = "FeedbackEmail address is required.")]
        [EmailAddress(ErrorMessage = "This isn't an FeedbackEmail address.")]
        public string FeedbackEmail { get; set; }

        [Required(ErrorMessage = "EmailOwn address is required.")]
        [EmailAddress(ErrorMessage = "This isn't an EmailOwn address.")]
        public string EmailOwn { get; set; }

        [Required(ErrorMessage = "Location is required!")]
        [StringLength(100, MinimumLength = 12, ErrorMessage = "The LocationId must be between 12 and 100 characters.")]
        public string LocationId { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "The Address must be between 4 and 100 characters.")]
        public string Address { get; set; }

    }
}
