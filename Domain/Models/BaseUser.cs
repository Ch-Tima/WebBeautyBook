using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class BaseUser : IdentityUser
    {
        public string UserSurname { get; set; }
        public string Photo { get; set; }

        public string? WorkerId { get; set; }
        public Worker? Worker { get; set; }

        public List<CompanyLike> FavoriteCompanies { get; set; }

        public BaseUser() : base()
        {
            FavoriteCompanies = new List<CompanyLike>();
        }

    }
}
