namespace Domain.Models
{
    public class Location
    {

        public string Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public List<Company> Companies { get; set; }

        public Location()
        {
            Id = Guid.NewGuid().ToString();
            Companies = new List<Company>();
        }
    }
}
