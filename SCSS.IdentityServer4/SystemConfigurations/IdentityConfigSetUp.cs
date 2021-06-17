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

            //var connectionString = "Data Source=scss-database.cehfzxl85v4h.ap-southeast-1.rds.amazonaws.com;Initial Catalog=SCSS-DB-IdentityServer4;User ID=admin;Password=scsspassword123";


            services.AddDbContext<IdentityDBContext>(config =>
            {
                config.UseSqlServer(AppSettingValues.SqlConnectionString);
            });


            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityDBContext>()
                    .AddDefaultTokenProviders();
        }
    }
}
