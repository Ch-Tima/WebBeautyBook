using DAL.Initializers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    /// <summary>
    /// The database context for the BeautyBook application.
    /// </summary>
    public class BeautyBookDbContext : IdentityDbContext<BaseUser, IdentityRole, string>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BeautyBookDbContext"/> class with the specified database options.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public BeautyBookDbContext(DbContextOptions<BeautyBookDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Configures the database schema and relationships between entities.
        /// </summary>
        /// <param name="builder">The model builder used to define the database schema.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {

            //BaseUser
            builder.Entity<BaseUser>().Property(b => b.UserSurname).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<BaseUser>().Property(b => b.Photo).HasColumnType("VARCHAR").HasMaxLength(200).HasDefaultValue("/images/profile.png").IsRequired(false);
            //One to one
            builder.Entity<BaseUser>().Property(b => b.WorkerId).IsRequired(false);
            builder.Entity<BaseUser>().HasOne(e => e.Worker).WithOne(e => e.BaseUser).HasForeignKey<Worker>(e => e.BaseUserId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            //Worker
            builder.Entity<Worker>().HasIndex(w => w.Id).IsUnique();
            //One to many
            builder.Entity<Worker>().HasOne(w => w.Company).WithMany(c => c.Workers).HasForeignKey(w => w.CompanyId).IsRequired();

            //Company
            builder.Entity<Company>().HasIndex(c => c.Id).IsUnique();
            builder.Entity<Company>().Property(c => c.Name).HasColumnType("VARCHAR").HasMaxLength(100).HasDefaultValue("Company_" + DateTime.Now.ToString("yyyy/MMM/ddd|hh:mm:ss"));
            builder.Entity<Company>().Property(c => c.Phonenumber).HasColumnType("VARCHAR").HasMaxLength(13).IsRequired(false);
            builder.Entity<Company>().Property(c => c.Email).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Company>().Property(c => c.Logo).HasColumnType("VARCHAR").HasMaxLength(200).HasDefaultValue("/images/logoCompany.png").IsRequired(false);
            builder.Entity<Company>().Property(c => c.Address).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Company>().Property(s => s.IsVisibility).HasColumnType("BIT").HasDefaultValue<bool>(false);

            //One to Many (Location)
            builder.Entity<Company>().HasOne(c => c.Location).WithMany(l => l.Companies).HasForeignKey(c => c.LocationId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            //ScheduleCompany
            builder.Entity<CompanyOpenHours>().HasIndex(s => s.Id).IsUnique();
            builder.Entity<CompanyOpenHours>().Property(s => s.OpenFrom).HasColumnType("Time").IsRequired();
            builder.Entity<CompanyOpenHours>().Property(s => s.OpenUntil).HasColumnType("Time").IsRequired();
            builder.Entity<CompanyOpenHours>().Property(s => s.DayOfWeek).HasColumnType("Tinyint").IsRequired();
            //One to Many
            builder.Entity<CompanyOpenHours>().HasOne(s => s.Company).WithMany(c => c.CompanyOpenHours).HasForeignKey(s => s.CompanyId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            //CompanyImage
            builder.Entity<CompanyImage>().HasIndex(ci => ci.Id).IsUnique();
            builder.Entity<CompanyImage>().Property(ci => ci.Path).HasColumnType("VARCHAR").HasMaxLength(500).IsRequired();
            builder.Entity<CompanyImage>().HasOne(ci => ci.Company).WithMany(c => c.CompanyImages).HasForeignKey(ci => ci.CompanyId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            //CompanyLike
            builder.Entity<CompanyLike>().HasIndex(cl => cl.Id).IsUnique();
            builder.Entity<CompanyLike>().HasOne(cl => cl.User).WithMany(u => u.FavoriteCompanies).HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CompanyLike>().HasOne(cl => cl.Company).WithMany(c => c.LikedByUsers).HasForeignKey(u => u.CompanyId).IsRequired().OnDelete(DeleteBehavior.Cascade);


            //Comment
            builder.Entity<Comment>().HasIndex(c => c.Id).IsUnique();
            builder.Entity<Comment>().Property(c => c.Text).HasColumnType("VARCHAR").HasMaxLength(500);
            builder.Entity<Comment>().Property(c => c.Star).HasColumnType("FLOAT");
            //One to one
            builder.Entity<Comment>().HasOne(c => c.Appointment).WithOne(r => r.Comment).HasForeignKey<Comment>(c => c.AppointmentId).OnDelete(DeleteBehavior.Cascade).IsRequired();


            //Appointment
            builder.Entity<Appointment>().HasIndex(a => a.Id).IsUnique();
            builder.Entity<Reservation>().Property(s => s.Date).HasColumnType("date").IsRequired();
            builder.Entity<Reservation>().Property(s => s.TimeStart).HasColumnType("Time").IsRequired();
            builder.Entity<Reservation>().Property(s => s.TimeEnd).HasColumnType("Time").IsRequired();
            builder.Entity<Appointment>().Property(a => a.Note).HasColumnType("VARCHAR").HasMaxLength(500);
            builder.Entity<Appointment>().Property(a => a.Status).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Appointment>().Property(a => a.CommentId).IsRequired(false);
            //One to many
            builder.Entity<Appointment>().HasOne(a => a.BaseUser).WithMany(u => u.Appointments).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            builder.Entity<Appointment>().HasOne(a => a.Service).WithMany(u => u.Appointments).HasForeignKey(a => a.ServiceId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Appointment>().HasOne(a => a.Worker).WithMany(u => u.Appointments).HasForeignKey(a => a.WorkerId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            //Reservation
            builder.Entity<Reservation>().HasIndex(s => s.Id).IsUnique();
            builder.Entity<Reservation>().Property(s => s.Date).HasColumnType("date").IsRequired();
            builder.Entity<Reservation>().Property(s => s.TimeStart).HasColumnType("Time").IsRequired();
            builder.Entity<Reservation>().Property(s => s.TimeEnd).HasColumnType("Time").IsRequired();
            builder.Entity<Reservation>().Property(s => s.Description).HasColumnType("VARCHAR").HasMaxLength(250).IsRequired(false);
            builder.Entity<Reservation>().HasOne(r => r.Worker).WithMany(w => w.Reservations).HasForeignKey(r => r.WorkerId).OnDelete(DeleteBehavior.Cascade);

            //Service
            builder.Entity<Service>().HasIndex(s => s.Id).IsUnique();
            builder.Entity<Service>().Property(s => s.Name).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Service>().Property(s => s.Description).HasColumnType("VARCHAR").HasMaxLength(500);
            builder.Entity<Service>().Property(s => s.Time).HasColumnType("Time");
            builder.Entity<Service>().Property(s => s.Price).HasColumnType("DECIMAL");
            //One to Many
            builder.Entity<Service>().HasOne(s => s.Company).WithMany(c => c.Services).HasForeignKey(s => s.CompanyId);
            //CategotyId OneToMany
            builder.Entity<Service>().HasOne(x => x.Category).WithMany(x => x.Services).HasForeignKey(x => x.CategoryId);


            //Assignment
            builder.Entity<Assignment>().HasKey(ws => new { ws.WorkerId, ws.ServiceId });
            builder.Entity<Assignment>().Property(ws => ws.IsBlock).HasColumnType("BIT").HasDefaultValue(false);
            //One to many
            builder.Entity<Assignment>().HasOne(ws => ws.Service).WithMany(s => s.Assignments).HasForeignKey(ws => ws.ServiceId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Assignment>().HasOne(ws => ws.Worker).WithMany(w => w.Assignments).HasForeignKey(ws => ws.WorkerId).IsRequired().OnDelete(DeleteBehavior.Restrict);

            //Location
            builder.Entity<Location>().HasIndex(l => l.Id).IsUnique();
            builder.Entity<Location>().Property(l => l.City).HasColumnType("VARCHAR").HasMaxLength(100);
            builder.Entity<Location>().Property(l => l.Country).HasColumnType("VARCHAR").HasMaxLength(100);

            //Categoty
            builder.Entity<Category>().HasIndex(x => x.Id).IsUnique();
            builder.Entity<Category>().Property(x => x.Name).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            builder.Entity<Category>().HasMany(x => x.Categories).WithOne(x => x.Categors).HasForeignKey(x => x.CategoryId);

            //Default Initialization
            BeautyBookDbInitializer.Build(builder).Initializer();

            base.OnModelCreating(builder);
        }

        // DbSet properties for entity access

        public DbSet<BaseUser> BaseUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CompanyImage> CompanyImages { get; set; }
        public DbSet<CompanyLike> CompanyLikes { get; set; }
    }
}
