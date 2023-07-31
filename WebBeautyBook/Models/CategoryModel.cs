using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class CategoryModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The \"Name\" must be between 3 and 100 characters.")]
        public string name { get; set; }

        public string? categoryId { get; set; }
    }
}
