using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class AppointmentViewModel
    {

        [Required(ErrorMessage = $"The {nameof(ForWhatTime)} is required.")]
        public DateTime ForWhatTime { get; set; }

        [StringLength(500, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string Note { get; set; }

        public string? UserId { get; set; }

        [Required(ErrorMessage = $"The {nameof(AssignmentId)} is required.")]
        public string AssignmentId { get; set; }

    }
}
