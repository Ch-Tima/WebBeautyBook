namespace Domain.Models
{
    public class Schedule
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public bool IsWork { get; set; }

        public string WorkerId { get; set; }
        public Worker Worker { get; set; }

        public List<Record> Records { get; set; }

        public Schedule()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
