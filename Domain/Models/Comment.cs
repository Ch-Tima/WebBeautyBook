namespace Domain.Models
{
    public class Comment
    {

        public string Id { get; set; }
        public string Text { get; set; }
        public float Star { get; set; }
        public string AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public Comment()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
