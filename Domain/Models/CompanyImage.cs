namespace Domain.Models
{
    public class CompanyImage
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public CompanyImage() 
        { 
            Id = Guid.NewGuid().ToString();
        }
    }
}
