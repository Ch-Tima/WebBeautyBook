using System.ComponentModel.DataAnnotations;

namespace WebBeautyBook.Models
{
    public class WorkerServiceModel
    {
        [Required(ErrorMessage = "WorkerId is required!")]
        public string workerId { get; set; }

        [Required(ErrorMessage = "SericeId is required!")]
        public string serviceId { get; set; }
    }
}
