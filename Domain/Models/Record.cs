namespace Domain.Models
{
    public class Record
    {
        public string Id { get; set; }
        //Time HH:mm:ss (23:59:59) 
        public TimeSpan ForWhatTime { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }


        public string ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public string ClientId { get; set; }
        public Client Client { get; set; }

        public string WorkerServiceId { get; set; }
        public WorkerService WorkerService { get; set; }

        public string CommentId { get; set; }
        public Comment Comment { get; set; }

        public Record()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
