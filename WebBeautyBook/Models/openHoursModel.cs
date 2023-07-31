using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class OpenHoursModel
    {
        [Required(ErrorMessage = "OpenFrom is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        public TimeOnly OpenFrom { get; set; }

        [Required(ErrorMessage = "OpenUntil is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        public TimeOnly OpenUntil { get; set; }

        [Required(ErrorMessage = "DayOfWeek is required!")]
        [Range(1, 7, ErrorMessage = "The DayOfWeek must between 1 and 7.")]
        public byte DayOfWeek { get; set; }
    }
}
