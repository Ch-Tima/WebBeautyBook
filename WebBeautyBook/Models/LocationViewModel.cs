using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class LocationViewModel
    {
        [Required(ErrorMessage = "City is required!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The \"City\" must be between 3 and 100 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The \"Country\" must be between 3 and 100 characters.")]
        public string Country { get; set; }
    }
}
