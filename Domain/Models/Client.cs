namespace Domain.Models
{
    public class Client
    {
        public string Id { get; set; }
        public string BaseUserId { get; set; }
        public BaseUser BaseUser { get; set; }
        public List<Record> Records { get; set; }

        public Client()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
