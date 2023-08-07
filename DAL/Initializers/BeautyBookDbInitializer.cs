using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Initializers
{
    public static class BeautyBookDbInitializer
    {
        private const string countryUkraine = "Ukraine";
        private const string countryPoland = "Poland";
        private const string countryGermany = "Germany";

        /// <summary>
        /// locationWarsaw use in the <see cref="BeautyBookDbInitializer.Companies">Companies</see>  method to create a company
        /// </summary>
        private static readonly Location locationWarsaw = new Location()
        {
            Country = countryPoland,
            City = "Warsaw"
        };

        private static readonly IdentityRole _adminRole = new IdentityRole()
        {
            Name = Roles.ADMIN,
            NormalizedName = Roles.ADMIN.ToUpper()
        };
        private static readonly IdentityRole _ownRole = new IdentityRole()
        {
            Name = Roles.OWN_COMPANY,
            NormalizedName = Roles.OWN_COMPANY.ToUpper()
        };

        public static void Initializer(ModelBuilder builder)
        {
            //Roles
            builder.Entity<IdentityRole>().HasData(new IdentityRole[]
            {
                new IdentityRole()
                {
                    Name = Roles.CLIENT,
                    NormalizedName = Roles.CLIENT.ToUpper()
                },
                new IdentityRole()
                {
                    Name = Roles.WORKER,
                    NormalizedName = Roles.WORKER.ToUpper()
                },
                new IdentityRole()
                {
                    Name = Roles.MANAGER,
                    NormalizedName = Roles.MANAGER.ToUpper()
                },
                _ownRole,
                _adminRole
            });

            //Other
            Loaction(builder);
            Category(builder);
            SupperAdmin(builder);
            FirstCompany(builder);
        }


        private static void SupperAdmin(ModelBuilder builder)
        {

            //hasher for password
            var hasher = new PasswordHasher<BaseUser>();

            //supperAdmin
            var supperAdmin = new BaseUser()
            {
                UserName = "Tima",
                NormalizedUserName = "Tima".ToUpper(),
                UserSurname = "Ch",
                NormalizedEmail = "chizhevskii.tima@gmail.com".ToUpper(),
                Email = "chizhevskii.tima@gmail.com",
                EmailConfirmed = true
            };
            supperAdmin.PasswordHash = hasher.HashPassword(supperAdmin, "admin");

            builder.Entity<BaseUser>().HasData(new BaseUser[] { supperAdmin });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>()
                {
                    UserId = supperAdmin.Id,
                    RoleId = _adminRole.Id
                }
            });

        }

        private static void FirstCompany(ModelBuilder builder)
        {

            //logologi417 own company
            var logologi417Email = "logologi417@gmail.com";
            var logologi417 = new BaseUser()
            {
                UserName = "Rochus",
                NormalizedUserName = "Rochus".ToUpper(),
                UserSurname = "Federico",
                NormalizedEmail = logologi417Email.ToUpper(),
                Email = logologi417Email,
                EmailConfirmed = true
            };
            var hasher = new PasswordHasher<BaseUser>();
            logologi417.PasswordHash = hasher.HashPassword(logologi417, logologi417Email);
            builder.Entity<BaseUser>().HasData(new BaseUser[] { logologi417 });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>[]{
                new IdentityUserRole<string>()
                {
                    UserId = logologi417.Id,
                    RoleId = _ownRole.Id
                }
            });
            

            //company
            var companyForLog417 = new Company()
            {
                Name = "EvilCompany",
                Email = logologi417.Email,
                Address = "Wspulna 92S",
                Phonenumber = "+48555111222",
                IsVisibility = false,
                LocationId = locationWarsaw.Id
            };
            builder.Entity<Company>().HasData(new Company[] { companyForLog417 });

            //worker profile
            var workerProfileForLog417 = new Worker()
            {
                BaseUserId = logologi417.Id,
                CompanyId = companyForLog417.Id,
            };
            logologi417.WorkerId = workerProfileForLog417.Id;
            builder.Entity<Worker>().HasData(new Worker[] { workerProfileForLog417 });

            //CompanyImage
            var img1 = new CompanyImage()
            {
                CompanyId = companyForLog417.Id,
                Path = "/images/image.png"
            };
            var img2 = new CompanyImage()
            {
                CompanyId = companyForLog417.Id,
                Path = "/images/test.png"
            };
            builder.Entity<CompanyImage>().HasData(new CompanyImage[] { img1, img2 });

            //CompanyOpenHours
            builder.Entity<CompanyOpenHours>().HasData(new CompanyOpenHours[]
            {
                new CompanyOpenHours()
                {
                    CompanyId = companyForLog417.Id,
                    DayOfWeek = 1,//Monday
                    OpenFrom = new TimeSpan(10, 0, 0),
                    OpenUntil = new TimeSpan(19, 0, 0)
                },
                //Tuesday close 2
                new CompanyOpenHours()
                {
                    CompanyId = companyForLog417.Id,
                    DayOfWeek = 3,//Wednesday
                    OpenFrom = new TimeSpan(10, 0, 0),
                    OpenUntil = new TimeSpan(19, 0, 0)
                },
                //Thursday close 4
                //Friday close 5
                new CompanyOpenHours()
                {
                    CompanyId = companyForLog417.Id,
                    DayOfWeek = 6,//Saturday
                    OpenFrom = new TimeSpan(10, 0, 0),
                    OpenUntil = new TimeSpan(16, 30, 0)
                },
                //Sunday close 7
            });
        }

        private static void Loaction(ModelBuilder builder)
        {
            //Locations
            builder.Entity<Location>().HasData(new Location[]
            {
                //Ukraine
                new Location()
                {
                    Country = countryUkraine,
                    City = "Kyiv"
                },
                new Location()
                {
                    Country = countryUkraine,
                    City = "Kharkiv"
                },
                new Location()
                {
                    Country = countryUkraine,
                    City = "Odesa"
                },
                new Location()
                {
                    Country = countryUkraine,
                    City = "Dnipro"
                },
                //Poland
                locationWarsaw,
                new Location()
                {
                    Country = countryPoland,
                    City = "Kraków"
                },
                new Location()
                {
                    Country = countryPoland,
                    City = "Łódź"
                },
                new Location()
                {
                    Country = countryPoland,
                    City = "Poznań"
                },
                new Location()
                {
                    Country = countryPoland,
                    City = "Wrocław"
                },
                //Germany
                new Location()
                {
                    Country = countryGermany,
                    City = "Berlin"
                },
                new Location()
                {
                    Country = countryGermany,
                    City = "Hamburg"
                },
                new Location()
                {
                    Country = countryGermany,
                    City = "Munich"
                },                
                new Location()
                {
                    Country = countryGermany,
                    City = "Cologne"
                },
            });
        }

        private static void Category(ModelBuilder builder)
        {
            //Categories
            var manicure = new Category() { Name = "Manicure" };
            var hairdressing = new Category() { Name = "Hairdressing" };
            var eye = new Category() { Name = "Eyebrows/Eyelashes" };
            var facial = new Category() { Name = "Facial" };

            builder.Entity<Category>().HasData(new Category[]
            {
                //facial
                facial,
                new Category()
                {
                    Name = "Facial cleansing",
                    CategoryId = facial.Id
                },
                new Category()
                {
                    Name = "Facial peeling",
                    CategoryId = facial.Id
                },
                new Category()
                {
                    Name = "Facial massage",
                    CategoryId = facial.Id
                },

                //eye
                eye,
                new Category()
                {
                    Name = "Eyelash tinting",
                    CategoryId = eye.Id
                },
                new Category()
                {
                    Name = "Eyelash extension",
                    CategoryId = eye.Id
                },
                //hairdressing
                hairdressing,
                new Category()
                {
                    Name = "Women's hairdressing",
                    CategoryId = hairdressing.Id
                },
                new Category()
                {
                    Name = "Men's hairdressing",
                    CategoryId = hairdressing.Id
                },
                new Category()
                {
                    Name = "Hair dyeing",
                    CategoryId = hairdressing.Id
                },
                //manicure
                manicure,
                new Category()
                {
                    Name = "Basic",
                    CategoryId = manicure.Id
                },
                new Category()
                {
                    Name = "French",
                    CategoryId = manicure.Id
                },
                new Category()
                {
                    Name = "Acrylic",
                    CategoryId = manicure.Id
                },
                new Category()
                {
                    Name = "Gel",
                    CategoryId = manicure.Id
                }
            });
        }
    }
}
