using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SCSS.IdentityServer4.Data.IdenittyEF;
using SCSS.IdentityServer4.Data.Identity;
using SCSS.Utilities.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.SystemConfigurations
{
    internal static class IdentityConfigSetUp
    {
        public static void AddIdentityConfigSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            var connectionString = "Data Source=scss-db-instance.cehfzxl85v4h.ap-southeast-1.rds.amazonaws.com;Initial Catalog=SCSS-DB-IdentityServer4;User ID=admin;Password=scsspassword123";


            services.AddDbContext<IdentityDBContext>(config =>
            {
                config.UseSqlServer(connectionString);
            });


            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityDBContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromDays(2)
                );
        }
    }
}
