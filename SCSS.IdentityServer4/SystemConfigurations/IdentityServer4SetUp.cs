using IdentityServer4.Configuration;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SCSS.IdentityServer4.Constants;
using SCSS.IdentityServer4.Data.Identity;
using SCSS.IdentityServer4.IdentityServerConfig;
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

            //var connectionString = "Data Source=scss-database.cehfzxl85v4h.ap-southeast-1.rds.amazonaws.com;Initial Catalog=SCSS-DB-IdentityServer4;User ID=admin;Password=scsspassword123";

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Endpoints.EnableUserInfoEndpoint = true;
                options.UserInteraction.LoginUrl = IdenittyUrlDefination.UserInteractionLoginUrl;
                options.UserInteraction.LogoutUrl = IdenittyUrlDefination.UserInteractionLoginUrl;
                //options.Authentication = new AuthenticationOptions()
                //{
                //    CookieLifetime = TimeSpan.FromHours(10), // ID server cookie timeout set to 10 hours
                //    CookieSlidingExpiration = true
                //};
            })
            .AddAspNetIdentity<ApplicationUser>()
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

            services.AddScoped<ICustomTokenRequestValidator, CustomTokenResponseGenerator>();
        }
    }
}
