using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SCSS.IdentityServer4.Enities;
using SCSS.Utilities.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.SystemConfigurations
{
    public static class IdentityServer4SetUp
    {
        public static void AddIdentityServer4SetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;


            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Endpoints.EnableUserInfoEndpoint = true;

            }).AddAspNetIdentity<ApplicationUser>()
            .AddDeveloperSigningCredential()
            .AddConfigurationStore(option =>
            {
                option.ConfigureDbContext = b => b.UseSqlServer(AppSettingValues.SqlConnectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(option =>
            {
                option.ConfigureDbContext = b => b.UseSqlServer(AppSettingValues.SqlConnectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            });
        }
    }
}
