namespace Domain.Models
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string? CategoryId { get; set; }
        public Category? Categors { get; set; }

        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Category> Categories { get; set; }


        public Category()
        {
            Id = Guid.NewGuid().ToString();
            Services = new List<Service>();
            Categories = new List<Category>();
        }

    }
}
