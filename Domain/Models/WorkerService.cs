namespace Domain.Models
{
    public class WorkerService
    {
        public string Id { get; set; }
        public bool IsBlock { get; set; }

        public string WorkerId { get; set; }
        public Worker Worker { get; set; }

        public string ServiceId { get; set; }
        public Service Service{ get; set; }

        public List<Record> Records { get; set; }

        public WorkerService()
        {
            Id = Guid.NewGuid().ToString();
            Records = new List<Record>();
        }
    }
}
