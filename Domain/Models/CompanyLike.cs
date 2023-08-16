namespace Domain.Models
{
    /// <summary>
    /// This class is an intermediate table in the database for <see cref="BaseUser.FavoriteCompanies">BaseUser</see> and <see cref="Company.LikedByUsers">Company</see>.
    /// <br>To create many-to-many connections.</br>
    /// </summary>
    public class CompanyLike
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public BaseUser User { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public CompanyLike() 
        { 
            Id = Guid.NewGuid().ToString();
        }
    }
}
