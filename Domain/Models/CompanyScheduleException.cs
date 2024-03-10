
namespace Domain.Models
{
    public class CompanyScheduleException
    {

        public string Id { get; set; }
        public DateTime ExceptionDate { get; set; }

        public TimeSpan OpenFrom { get; set; }
        public TimeSpan OpenUntil { get; set; }

        public bool IsClosed { get; set; }

        public bool IsOnce { get; set; }

        public string Reason { get; set; }

        public Company Company { get; set; }
        public string CompanyId { get; set; }

        public CompanyScheduleException() 
        {

            Id = Guid.NewGuid().ToString();

        }

    }
}
