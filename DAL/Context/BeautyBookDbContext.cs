using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace DAL.Context
{
    public class BeautyBookDbContext : IdentityDbContext<BaseUser, IdentityRole, string>
    {
        public BeautyBookDbContext(DbContextOptions<BeautyBookDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //BaseUser
            builder.Entity<BaseUser>().Property<string>(b => b.UserSurname).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<BaseUser>().Property<string>(b => b.Photo).HasColumnType("VARCHAR").HasMaxLength(200)
                .HasDefaultValue("/wwwroot/images/profile.png").IsRequired(false);
            //One to one
            builder.Entity<BaseUser>().Property(b => b.WorkerId).IsRequired(false);
            builder.Entity<BaseUser>().HasOne(e => e.Worker).WithOne(e => e.BaseUser).HasForeignKey<Worker>(e => e.BaseUserId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            //Worker
            builder.Entity<Worker>().HasIndex(w => w.Id).IsUnique();
            //One to many
            builder.Entity<Worker>().HasOne(w => w.Company).WithMany(c => c.Workers).HasForeignKey(w => w.CompanyId).IsRequired();

            //Client
            builder.Entity<Client>().HasIndex(b => b.Id).IsUnique();


            //Company
            builder.Entity<Company>().HasIndex(c => c.Id).IsUnique();
            builder.Entity<Company>().Property(c => c.Name).HasColumnType("VARCHAR").HasMaxLength(100).HasDefaultValue("Company_" + DateTime.Now.ToString("yyyy/MMM/ddd|hh:mm:ss"));
            builder.Entity<Company>().Property(c => c.Phonenumber).HasColumnType("VARCHAR").HasMaxLength(13).IsRequired(false);
            builder.Entity<Company>().Property(c => c.Email).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Company>().Property(c => c.Logo).HasColumnType("VARCHAR").HasMaxLength(200).HasDefaultValue("/wwwroot/images/logoCompany.png").IsRequired(false);
            builder.Entity<Company>().Property(c => c.Address).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Company>().Property(s => s.IsVisibility).HasColumnType("BIT").HasDefaultValue<bool>(false);

            //One to Many (Location)
            builder.Entity<Company>().HasOne(c => c.Location).WithMany(l => l.Companies).HasForeignKey(c => c.LocationId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);


            //Comment
            builder.Entity<Comment>().HasIndex(c => c.Id).IsUnique();
            builder.Entity<Comment>().Property(c => c.Text).HasColumnType("VARCHAR").HasMaxLength(500);
            builder.Entity<Comment>().Property(c => c.Star).HasColumnType("FLOAT");
            //One to one
            builder.Entity<Comment>().HasOne(c => c.Record).WithOne(r => r.Comment).HasForeignKey<Comment>(c => c.RecordId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Comment>().Property(c => c.RecordId).IsRequired();


            //Recodr
            builder.Entity<Record>().HasIndex(r => r.Id).IsUnique();
            builder.Entity<Record>().Property(r => r.ForWhatTime).HasColumnType("Time");
            builder.Entity<Record>().Property(r => r.Note).HasColumnType("VARCHAR").HasMaxLength(500);
            builder.Entity<Record>().Property(r => r.Status).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Record>().Property(r => r.CommentId).IsRequired(false);
            //One to many
            builder.Entity<Record>().HasOne(r => r.Schedule).WithMany(s => s.Records).HasForeignKey(r => r.ScheduleId).OnDelete(DeleteBehavior.NoAction);//?
            builder.Entity<Record>().HasOne(r => r.Client).WithMany(s => s.Records).HasForeignKey(r => r.ClientId);
            builder.Entity<Record>().HasOne(r => r.WorkerService).WithMany(s => s.Records).HasForeignKey(r => r.WorkerServiceId);


            //Schedule
            builder.Entity<Schedule>().HasIndex(s => s.Id).IsUnique();
            builder.Entity<Schedule>().Property(s => s.Date).HasColumnType("date");
            builder.Entity<Schedule>().Property(s => s.TimeStart).HasColumnType("Time");
            builder.Entity<Schedule>().Property(s => s.TimeEnd).HasColumnType("Time");
            builder.Entity<Schedule>().Property(s => s.IsWork).HasColumnType("BIT").HasDefaultValue<bool>(false);


            //Service
            builder.Entity<Service>().HasIndex(s => s.Id).IsUnique();
            builder.Entity<Service>().Property(s => s.Name).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Service>().Property(s => s.Description).HasColumnType("VARCHAR").HasMaxLength(500);
            builder.Entity<Service>().Property(s => s.Time).HasColumnType("Time");
            builder.Entity<Service>().Property(s => s.Price).HasColumnType("DECIMAL");
            //One to Many
            builder.Entity<Service>().HasOne(s => s.Company).WithMany(c => c.Services).HasForeignKey(s => s.CompanyId);
            //CategotyId OneToMany
            builder.Entity<Service>().HasOne(x => x.Categoty).WithMany(x => x.Services).HasForeignKey(x => x.CategotyId);


            //WorkerService
            builder.Entity<WorkerService>().HasIndex(ws => ws.Id).IsUnique();
            builder.Entity<WorkerService>().Property(ws => ws.IsBlock).HasColumnType("BIT").HasDefaultValue<bool>(false);
            //One to many
            builder.Entity<WorkerService>().HasOne(ws => ws.Service).WithMany(s => s.WorkerServices).HasForeignKey(ws => ws.ServiceId).OnDelete(DeleteBehavior.NoAction);//?
            builder.Entity<WorkerService>().HasOne(ws => ws.Worker).WithMany(w => w.WorkerServices).HasForeignKey(ws => ws.WorkerId).OnDelete(DeleteBehavior.NoAction);//?

            //Location
            builder.Entity<Location>().HasIndex(l => l.Id).IsUnique();
            builder.Entity<Location>().Property<string>(l => l.City).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Location>().Property<string>(l => l.Country).HasColumnType("VARCHAR").HasMaxLength(100);

            //Categoty
            builder.Entity<Category>().HasIndex(x => x.Id).IsUnique();
            builder.Entity<Category>().Property(x => x.Name).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            builder.Entity<Category>().HasMany(x => x.Categories).WithOne(x => x.Categors).HasForeignKey(x => x.CategoryId);

            //Default Initialization
            //Roles
            var adminRole = new IdentityRole()
            {
                Name = Domain.Models.Roles.ADMIN,
                NormalizedName = Domain.Models.Roles.ADMIN.ToUpper()
            };
            builder.Entity<IdentityRole>().HasData(new IdentityRole[]
            {
                new IdentityRole()
                {
                    Name = Domain.Models.Roles.CLIENT,
                    NormalizedName = Domain.Models.Roles.CLIENT.ToUpper()
                },
                new IdentityRole()
                {
                    Name = Domain.Models.Roles.WORKER,
                    NormalizedName = Domain.Models.Roles.WORKER.ToUpper()
                },
                new IdentityRole()
                {
                    Name = Domain.Models.Roles.MANAGER,
                    NormalizedName = Domain.Models.Roles.MANAGER.ToUpper()
                },
                new IdentityRole()
                {
                    Name = Domain.Models.Roles.OWN_COMPANY,
                    NormalizedName = Domain.Models.Roles.OWN_COMPANY.ToUpper()
                },
                adminRole
            });

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
            var hasher = new PasswordHasher<BaseUser>();
            supperAdmin.PasswordHash = hasher.HashPassword(supperAdmin, "admin");

            builder.Entity<BaseUser>().HasData(new BaseUser[] { supperAdmin });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>()
                {
                    UserId = supperAdmin.Id,
                    RoleId = adminRole.Id
                }
            });

            //Locations
            const string countryUkraine = "Ukraine";
            const string countryPoland = "Poland";
            builder.Entity<Location>().HasData(new Location[]
            {
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
                    Country = countryPoland,
                    City = "Warsaw"
                },
                new Location()
                {
                    Country = countryPoland,
                    City = "Kraków"
                }
            });

            //Categories
            var categoryManicure = new Category() { Name = "Manicure" };
            builder.Entity<Category>().HasData(new Category[]
            {
                new Category()
                {
                    Name = "Women's hairdressing"
                },
                new Category()
                {
                    Name = "Men's hairdressing"
                },
                categoryManicure,
                new Category()
                {
                    Name = "Basic",
                    CategoryId = categoryManicure.Id
                },
                new Category()
                {
                    Name = "French",
                    CategoryId = categoryManicure.Id
                },
                new Category()
                {
                    Name = "Acrylic",
                    CategoryId = categoryManicure.Id
                },
                new Category()
                {
                    Name = "Gel",
                    CategoryId = categoryManicure.Id
                }
            });

        }

        public DbSet<BaseUser> BaseUsers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<WorkerService> WorkersService { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
