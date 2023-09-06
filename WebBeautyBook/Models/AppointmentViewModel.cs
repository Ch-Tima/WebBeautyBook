using System.ComponentModel.DataAnnotations;
using WebBeautyBook.Validations;

namespace WebBeautyBook.Models
{
    public class AppointmentViewModel
    {
        [Required(ErrorMessage = $"The {nameof(Date)} is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTimeOffset Date { get; set; }

        [Required(ErrorMessage = $"{nameof(Time)} is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        public TimeOnly Time { get; set; }

        [StringLength(500, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string Note { get; set; }

        [Required(ErrorMessage = $"The {nameof(ServiceId)} is required.")]
        public string ServiceId { get; set; }

        [Required(ErrorMessage = $"The {nameof(WorkerId)} is required.")]
        public string WorkerId { get; set; }


    }
}
