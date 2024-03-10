using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class CompanyScheduleExceptionViewModel
    {


        [Required(ErrorMessage = $"The {nameof(ExceptionDate)} is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTimeOffset ExceptionDate { get; set; }

        [Required(ErrorMessage = "OpenFrom is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        public TimeOnly OpenFrom { get; set; }

        [Required(ErrorMessage = "OpenUntil is required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{HH:mm}")]
        public TimeOnly OpenUntil { get; set; }

        public bool IsClosed { get; set; } = true;
        public bool IsOnce { get; set; } = true;


        [MaxLength(100, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? Reason { get; set; }
    }
}
