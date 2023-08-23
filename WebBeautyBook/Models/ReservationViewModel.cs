using System.ComponentModel.DataAnnotations;
using WebBeautyBook.Validations;

namespace WebBeautyBook.Models
{
    public class ReservationViewModel
    {
        [Required(ErrorMessage = $"The {nameof(Date)} is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTimeOffset Date { get; set; }

        [Required(ErrorMessage = $"{nameof(TimeStart)} is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        public TimeOnly TimeStart { get; set; }


        [Required(ErrorMessage = $"{nameof(TimeEnd)} is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        [DifferentTimes(nameof(TimeStart), ErrorMessage = $"{nameof(TimeStart)} and {nameof(TimeEnd)} cannot be the same.")]
        public TimeOnly TimeEnd { get; set; }

        [MaxLength(500, ErrorMessage = $"The {nameof(Description)} must contain no more than 500 characters.")]
        public string? Description { get; set; }
    }
}
