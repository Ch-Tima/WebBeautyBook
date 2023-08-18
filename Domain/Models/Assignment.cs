namespace Domain.Models
{
    /// <summary>
    /// Assignment is an intermediate model for <see cref="Models.Worker"/> and <see cref="Models.Service"/>.<br/>
    /// To create a many-to-many relationship.
    /// </summary>
    public class Assignment
    {
        public bool IsBlock { get; set; }

        public string WorkerId { get; set; }
        public Worker Worker { get; set; }

        public string ServiceId { get; set; }
        public Service Service{ get; set; }

        public List<Appointment> Appointments { get; set; }

        public Assignment()
        {
            Appointments = new List<Appointment>();
        }
    }
}
