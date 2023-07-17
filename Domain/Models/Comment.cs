namespace Domain.Models
{
    public class Comment
    {

        public string Id { get; set; }
        public string Text { get; set; }
        public float Star { get; set; }
        public string RecordId { get; set; }
        public Record Record { get; set; }

        public Comment()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
