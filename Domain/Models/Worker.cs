namespace Domain.Models
{
    public class Worker
    {

        public string Id { get; set; }

        public string BaseUserId { get; set; }
        public BaseUser BaseUser { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public List<Assignment> Assignments { get; set; }
        public List<Reservation> Reservations { get; set; }

        public Worker()
        {
            Id = Guid.NewGuid().ToString();
            Assignments = new List<Assignment>();
            Reservations = new List<Reservation>();
        }
    }
}
