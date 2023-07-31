namespace Domain.Models
{
    public class CompanyOpenHours
    {
        public string Id { get; set; }
        
        public TimeSpan OpenFrom { get; set; }
        public TimeSpan OpenUntil { get; set; }
        public byte DayOfWeek { get; set; }

        public Company Company { get; set; }
        public string CompanyId { get; set; }

        public CompanyOpenHours()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

}
