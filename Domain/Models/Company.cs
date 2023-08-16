namespace Domain.Models
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public bool IsVisibility { get; set; }

        public string LocationId { get; set; }
        public Location Location { get; set; }

        public List<Worker> Workers { get; set; }
        public List<Service> Services { get; set; }
        public List<CompanyOpenHours> CompanyOpenHours { get; set; }
        public List<CompanyImage> CompanyImages { get; set; }
        public List<CompanyLike> LikedByUsers { get; set; }

        public Company()
        {
            Id = Guid.NewGuid().ToString();
            Workers = new List<Worker>();
            Services = new List<Service>();
            CompanyOpenHours = new List<CompanyOpenHours>();
            CompanyImages = new List<CompanyImage>();
            LikedByUsers = new List<CompanyLike>();
        }

    }
}
