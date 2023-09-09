using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;


namespace DAL.Initializers
{
    public class BeautyBookDbInitializer
    {
        private readonly ModelBuilder _modelBuilder;
        private readonly Random _random;

        private const string countryUkraine = "Ukraine";
        private const string countryPoland = "Poland";
        private const string countryGermany = "Germany";

        private List<string> firstNames = new List<string>(new[] { "John", "Mary", "David", "Anna", "Michael", "Alice", "Bob", "Catherine", "Daniel", "Ella", "Benito", "Salomón", "Rubén", "Sonia", "Heriber" });
        private List<string> lastNames = new List<string>(new[] { "Smith", "Johnson", "Brown", "Davis", "Williams", "Johnson", "Williams", "Smith", "Brown", "Davis", "Thomas", "Nathan", "Taylor", "Chris", "Herbert" });

        private List<string> beautySalonNames = new List<string>
        {
            "Radiant Elegance Beauty Salon", "Serene Splendor Beauty Studio", "Glamour Glow Beauty Hub", "Opulent Oasis Beauty Spa", "Chic Charm Beauty Boutique",
            "Luxe Look Salon & Spa", "Enchanted Beauty Oasis","Ethereal Essence Beauty Lounge", "Allure & Grace Beauty Emporium", "Tranquil Beauty Retreat",
            "Graceful Glamour Gallery", "Serenity Spa and Beauty", "Radiant Reflections Salon", "Luminous Luxury Beauty Bar", "Timeless Beauty Oasis"
        };

        private List<Location> locations = new List<Location>();
        private Dictionary<string, IdentityRole> roles = new Dictionary<string, IdentityRole>();
        private List<Category> mainCategory = new List<Category>();
        private List<Category> subCategory = new List<Category>();

        public BeautyBookDbInitializer(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
            _random = new Random();
        }

        public static BeautyBookDbInitializer Build(ModelBuilder modelBuilder)
        {
            return new BeautyBookDbInitializer(modelBuilder);
        }

        public void Initializer()
        {
            RolesInit();
            Loaction();
            Category();

            //always last!
            SupperAdmin();
            beautySalonNames.ForEach(item => CreateRandomCompany(item));
        }

        private void RolesInit()
        {
            roles.Add(Roles.ADMIN, new IdentityRole()
            {
                Name = Roles.ADMIN,
                NormalizedName = Roles.ADMIN.ToUpper()
            });
            roles.Add(Roles.OWN_COMPANY, new IdentityRole()
            {
                Name = Roles.OWN_COMPANY,
                NormalizedName = Roles.OWN_COMPANY.ToUpper()
            });
            roles.Add(Roles.MANAGER, new IdentityRole()
            {
                Name = Roles.MANAGER,
                NormalizedName = Roles.MANAGER.ToUpper()
            });
            roles.Add(Roles.WORKER, new IdentityRole()
            {
                Name = Roles.WORKER,
                NormalizedName = Roles.WORKER.ToUpper()
            });
            roles.Add(Roles.CLIENT, new IdentityRole()
            {
                Name = Roles.CLIENT,
                NormalizedName = Roles.CLIENT.ToUpper()
            });
            _modelBuilder.Entity<IdentityRole>().HasData(roles.Values.ToArray());
        }

        private void SupperAdmin()
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
            _modelBuilder.Entity<BaseUser>().HasData(new BaseUser[] { supperAdmin });
            _modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>()
                {
                    UserId = supperAdmin.Id,
                    RoleId = roles.GetValueOrDefault(Roles.ADMIN).Id
                }
            });

        }

        private void Loaction()
        {
            var citiesUkrain = new List<string>(new[] { "Kyiv", "Kharkiv", "Odesa", "Kharkiv", "Odesa", "Dnipro" });
            var citiesPoland = new List<string>(new[] { "Warsaw", "Kraków", "Łódź", "Poznań", "Wrocław" });
            var citiesGermany = new List<string>(new[] { "Berlin", "Hamburg", "Munich", "Cologne" });

            citiesUkrain.ForEach(item => locations.Add(new Location()
            {
                Country = countryUkraine, 
                City = item
            }));
            citiesPoland.ForEach(item => locations.Add(new Location()
            {
                Country = countryPoland,
                City = item
            }));
            citiesGermany.ForEach(item => locations.Add(new Location()
            {
                Country = countryGermany,
                City = item
            }));

            _modelBuilder.Entity<Location>().HasData(locations);
        }

        private void Category()
        {
            //Main Categories
            var manicure = new Category() { Name = "Manicure" };
            var hairdressing = new Category() { Name = "Hairdressing" };
            var eye = new Category() { Name = "Eyebrows/Eyelashes" };
            var facial = new Category() { Name = "Facial" };
            mainCategory = new List<Category>(new[] { manicure, hairdressing, eye, facial });
            //subcategories
            subCategory = new List<Category>(new Category[]
            {
                //facial
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
            //all categories
            var сategories = new List<Category>(mainCategory);
            сategories.AddRange(subCategory);
            _modelBuilder.Entity<Category>().HasData(сategories);
        }

        private void CreateRandomCompany(string name)
        {
            var ownRole = roles.GetValueOrDefault(Roles.OWN_COMPANY);
            var workerRole = roles.GetValueOrDefault(Roles.WORKER);
            if (ownRole == null || workerRole == null) return;

            //generate company owner
            var owner = CrateRandomUser();

            //create company
            var company = new Company()
            {
                Name = name,
                Email = owner.Email,
                Address = GenerateRandomAddress(),
                Phonenumber = GenerateRandomPhoneNumber(),
                IsVisibility = false,
                LocationId = locations.ToList()[new Random().Next(0, locations.Count)].Id
            };

            //CompanyImage
            var countImages = _random.Next(1, 3);
            var images = new List<CompanyImage>();
            for (int i = 0; i < countImages; i++)
            {
                images.Add(new CompanyImage()
                {
                    CompanyId = company.Id,
                    Path = $"/images/testData/{_random.Next(0, 9)}.jpg"
                });
            }

            //save company
            _modelBuilder.Entity<Company>().HasData(company);
            _modelBuilder.Entity<CompanyOpenHours>().HasData(GenerateRandomOpenHours(company.Id));
            _modelBuilder.Entity<CompanyImage>().HasData(images);

            //'worker' for company owner
            var ownerWorkerProfile = new Worker()
            {
                BaseUserId = owner.Id,
                CompanyId = company.Id
            };
            var userRoleOwner = new IdentityUserRole<string>()
            {
                RoleId = ownRole.Id,
                UserId = owner.Id
            };
            owner.WorkerId = ownerWorkerProfile.Id;
            _modelBuilder.Entity<BaseUser>().HasData(owner);
            _modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoleOwner);

            //generate employees
            var employees = CrateRandomUser(count: new Random().Next(1, 5));
            var workerProfiles = new List<Worker>();
            workerProfiles.Add(ownerWorkerProfile);
            var userRoleWorker = new List<IdentityUserRole<string>>();
            employees.ForEach(item =>
            {
                //profile
                var wp = new Worker()
                {
                    BaseUserId = item.Id,
                    CompanyId = company.Id
                };
                workerProfiles.Add(wp);
                item.WorkerId = wp.Id;
                //role
                userRoleWorker.Add(new IdentityUserRole<string>()
                {
                    RoleId = workerRole.Id,
                    UserId = item.Id
                });
            });
            _modelBuilder.Entity<BaseUser>().HasData(employees);
            _modelBuilder.Entity<Worker>().HasData(workerProfiles);
            _modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoleWorker);

            var service = GenerateRandomServices(company.Id);
            _modelBuilder.Entity<Service>().HasData(service);
            _modelBuilder.Entity<Assignment>().HasData(GenerateRandomAssignment(workerProfiles, service));
        }

        private List<Service> GenerateRandomServices(string companyId)
        {
            var services = new List<Service>();
            int numberOfServices = _random.Next(3, 6);

            for (int i = 0; i < numberOfServices; i++)
            {
                var category = subCategory[_random.Next(subCategory.Count)];
                var service = new Service
                {
                    Name = category.Name,
                    CategoryId = category.Id, // Select a random CategoryId from the array
                    Time = TimeSpan.FromMinutes(_random.Next(20, 121)), // Generate a random time between 20 minutes and 2 hours
                    Price = (decimal)_random.Next(10, 151), // Generate a random price between 10 and 200
                    CompanyId = companyId
                };
                service.Description = $"{category.Name} time:{service.Time.ToString(@"mm\:hh")} price:{service.Price}";
                services.Add(service);
            }

            return services;
        }

        private List<Assignment> GenerateRandomAssignment(List<Worker> workers, List<Service> services)
        {
            var assignments = new List<Assignment>();
            workers.ForEach(worker =>
            {
                int numberOfServices = new Random().Next(1, services.Count);
                var randomServices = services.OrderBy(s => Guid.NewGuid()).Take(numberOfServices);
                foreach (var service in randomServices)
                {
                    assignments.Add(new Assignment
                    {
                        WorkerId = worker.Id,
                        ServiceId = service.Id,
                    });
                }
            });
            return assignments;
        }

        private BaseUser CrateRandomUser()
        {
            var guid = Guid.NewGuid().ToString();
            string firstname = firstNames[_random.Next(0, firstNames.Count)];
            string lastname = lastNames[_random.Next(0, firstNames.Count)];
            return CreateUser(firstname + guid.Substring(0, 3), lastname + guid.Substring(3, 2));
        }

        private List<BaseUser> CrateRandomUser(int count)
        {
            List<BaseUser> users = new List<BaseUser>();
            for (int i = 0; i < count; i++)
                users.Add(CrateRandomUser());

            return users;
        }

        private BaseUser CreateUser(string firstname, string lastname, string? email = null, string? passwor = null)
        {
            if (email == null) email = $"{firstname}.{lastname}@gmail.com";
            var user = new BaseUser() 
            {
                UserName = firstname,
                NormalizedUserName = firstname.ToUpper(),
                UserSurname = lastname,
                NormalizedEmail = email.ToUpper(),
                Email = email,
                EmailConfirmed = true
            };
            var hasher = new PasswordHasher<BaseUser>();
            user.PasswordHash = hasher.HashPassword(user, passwor != null ? passwor : email);
            return user;
        }

        private string GenerateRandomPhoneNumber()
        {
            // Generate random area code (e.g., 3 digits)
            string areaCode = _random.Next(100, 1000).ToString();
            // Generate random exchange code (e.g., 3 digits)
            string exchangeCode = _random.Next(100, 1000).ToString();
            // Generate random subscriber number (e.g., 4 digits)
            string subscriberNumber = _random.Next(1000, 10000).ToString();
            // Format the phone number with dashes or other desired format
            string phoneNumber = $"{areaCode}-{exchangeCode}-{subscriberNumber}";
            return phoneNumber;
        }

        private string GenerateRandomAddress()
        {
            // Generate a random street name
            string[] streetNames = { "Main Street", "Oak Avenue", "Maple Road", "Cedar Lane", "Elm Street", "Sunset Boulevard", "Willow Street", "Pine Avenue", "Cypress Lane", "Meadowbrook Drive" };
            string streetName = streetNames[_random.Next(streetNames.Length)];

            // Generate a random state abbreviation
            string[] states = { "NY", "CA", "IL", "FL", "TX" };
            string state = states[_random.Next(states.Length)];

            // Combine the components into the address string
            string address = $"{_random.Next(1, 1000)} {streetName}, {state} {_random.Next(10000, 100000).ToString()}";

            return address;
        }
        
        private List<CompanyOpenHours> GenerateRandomOpenHours(string compayId)
        {
            var openHoursList = new List<CompanyOpenHours>();
            var uniqueDays = new HashSet<byte>();
            // Generate a random number of unique days of the week (from 4 to 7).
            while (uniqueDays.Count < _random.Next(4, 8))
            {
                byte dayOfWeek = (byte)_random.Next(0, 7); // 0 - Sunday, 6 - Saturday
                uniqueDays.Add(dayOfWeek);
            }

            foreach (byte dayOfWeek in uniqueDays)
            {
                var openHours = new CompanyOpenHours
                {
                    OpenFrom = TimeSpan.FromHours(_random.Next(6, 10)), // Generate a random opening time (from 6:00 AM to 11:59 AM).
                    OpenUntil = TimeSpan.FromHours(_random.Next(14, 21)), // Generate a random closing time (from 2:00 PM to 7:59 PM).
                    DayOfWeek = dayOfWeek,
                    CompanyId = compayId
                };
                openHoursList.Add(openHours);
            }

            return openHoursList;
        }

    }
}
