using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class ServiceModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The \"Name\" must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "The \"Description\" must contain no more than 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Time is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        public TimeOnly Time { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The price must be positive only.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Categoty is required!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The \"Categoty\" must be between 3 and 100 characters.")]
        public string CategoryId { get; set; }
    }
}
