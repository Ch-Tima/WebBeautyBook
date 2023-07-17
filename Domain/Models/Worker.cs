namespace Domain.Models
{
    public class Worker
    {

        public string Id { get; set; }

        public string BaseUserId { get; set; }
        public BaseUser BaseUser { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<WorkerService> WorkerServices { get; set; }

        public Worker()
        {
            Id = Guid.NewGuid().ToString();
            Schedules = new List<Schedule>();
            WorkerServices = new List<WorkerService>();
        }
    }
}
