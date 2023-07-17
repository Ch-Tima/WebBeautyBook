namespace Domain.Models
{
    public class Service
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        public decimal Price { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }
       
        public List<WorkerService>  WorkerServices { get; set; }

        public string CategotyId { get; set; }
        public Category? Categoty { get; set; }


        public Service()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
