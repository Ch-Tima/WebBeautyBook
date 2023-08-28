namespace Domain.Models
{
    /// <summary>
    /// Reservation - can only be created by an <see cref="Domain.Models.Worker"/>, for the created vacation or break.
    /// </summary>
    public class Reservation
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public string Description { get; set; }

        public string WorkerId { get; set; }
        public Worker Worker { get; set; }

        public Reservation()
        {
            Id = Guid.NewGuid().ToString();      
        }

    }
}
