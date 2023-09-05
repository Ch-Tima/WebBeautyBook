using BLL.Providers;
using DAL.Context;
using DAL.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

namespace BLL.Infrastructure
{
    /// <summary>
    /// Contains configuration methods for setting up services in the Business Logic Layer (BLL).
    /// </summary>
    public static class ConfigrationBLL
    {
        /// <summary>
        /// Configures services for the BLL based on the provided <paramref name="configuration"/>.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The configuration used for configuring services.</param>
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Configuration
            var local = configuration.GetConnectionString("local");//appsettings
            //var conMain = configuration.GetValue<string>("DbConection");//Azure secret key
            services.AddDbContext<BeautyBookDbContext>(opt => opt.UseSqlServer(local));

            // Add Repository Services
            services.AddTransient<BaseUserRepository>();
            services.AddTransient<CommentRepository>();
            services.AddTransient<LocationRepository>();
            services.AddTransient<AppointmentRepository>();
            services.AddTransient<ReservationRepository>();
            services.AddTransient<ServiceRepository>();
            services.AddTransient<AssignmentRepository>();
            services.AddTransient<WorkerRepository>();
            services.AddTransient<CategoryRepository>();
            services.AddTransient<CompanyRepository>();
            services.AddTransient<CompanyOpenHoursRepository>();
            services.AddTransient<CompanyImagesRepository>();
            services.AddTransient<CompanyLikeRepository>();

            services.AddMemoryCache();

            // Set settings Identity
            services.AddIdentity<BaseUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.AllowedUserNameCharacters += " ";

            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BeautyBookDbContext>()
            .AddDefaultTokenProviders()
            .AddInvitationToCompanyTokenProvider();

            //JWT Authentication Configuration
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Issuer"],
                    ValidIssuer = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                };
            });

            //TimeZone Configuration
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US") };
                options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("en-US") };
                options.RequestCultureProviders.Clear();
            });

        }

        /// <summary>
        /// Adds an invitation token provider to the IdentityBuilder for invitation-based company access.
        /// </summary>
        /// <param name="builder">The IdentityBuilder instance.</param>
        /// <returns>The updated IdentityBuilder with the invitation token provider.</returns>
        public static IdentityBuilder AddInvitationToCompanyTokenProvider(this IdentityBuilder builder) => builder.AddTokenProvider(
            InvitationToCompanyTokenProviderOptions.TokenProvider, 
            typeof(InvitationToCompanyTokenProvider<>).MakeGenericType(builder.UserType));
    }
}