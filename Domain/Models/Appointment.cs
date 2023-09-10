namespace Domain.Models
{
    /// <summary>
    /// Appointment - can only be created by a user or worker.<br/>
    /// But the <see cref="Appointment"/> time can be the same as the <see cref="Reservation"/> time when the worker permits.
    /// </summary>
    public class Appointment
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public string Note { get; set; }
        public AppointmentStatus Status { get; set; }

        public string UserId { get; set; }
        public BaseUser BaseUser { get; set; }

        public string WorkerId { get; set; }
        public Worker Worker { get; set; }

        public string ServiceId { get; set; }
        public Service Service { get; set; }

        public string CommentId { get; set; }
        public Comment Comment { get; set; }

        public Appointment()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
