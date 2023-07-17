using DAL.Context;
using DAL.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BLL.Infrastructure
{
    public static class ConfigrationBLL
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            var local = configuration.GetConnectionString("local");//appsettings
            //var conMain = configuration.GetValue<string>("DbConection");//Azure secret key
            services.AddDbContext<BeautyBookDbContext>(opt => opt.UseSqlServer(local));


            //Add Repository
            services.AddTransient<BaseUserRepository>();
            services.AddTransient<ClientRepository>();
            services.AddTransient<CommentRepository>();
            services.AddTransient<LocationRepository>();
            services.AddTransient<RecordRepository>();
            services.AddTransient<ScheduleRepository>();
            services.AddTransient<ServiceRepository>();
            services.AddTransient<WorkerServiceRepository>();
            services.AddTransient<WorkerRepository>();
            services.AddTransient<CategoryRepository>();
            services.AddTransient<CompanyRepository>();

            services.AddMemoryCache();

            //Set settings Identity
            services.AddDefaultIdentity<BaseUser>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.AllowedUserNameCharacters += " ";

            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<BeautyBookDbContext>()
            .AddDefaultTokenProviders();


            //JWT
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

        }
    }
}