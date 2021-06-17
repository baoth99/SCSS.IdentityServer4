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


            services.AddDbContext<IdentityDBContext>(config =>
            {
                config.UseSqlServer(AppSettingValues.IndentitySqlConnectionString);
            });


            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityDBContext>()
                    .AddDefaultTokenProviders();
        }
    }
}
