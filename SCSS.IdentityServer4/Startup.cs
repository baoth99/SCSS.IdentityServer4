using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SCSS.IdentityServer4.AuthenFilter;
using SCSS.IdentityServer4.Services.Implementations;
using SCSS.IdentityServer4.Services.Interfaces;
using SCSS.IdentityServer4.SystemConfigurations;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Helper;
using System;
using System.Net;
using Twilio;

namespace SCSS.IdentityServer4
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Enviroment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
        {
            Configuration = configuration;
            Enviroment = enviroment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region Configuration Helper

            services.AddSingleton(Configuration);
            ConfigurationHelper.Configuration = Configuration;
            ConfigurationHelper.IsDevelopment = Enviroment.IsDevelopment();
            ConfigurationHelper.IsTesting = Enviroment.EnvironmentName.Equals("Testing");
            ConfigurationHelper.IsProduction = Enviroment.IsProduction();
            FileHelper.ContentRootPath = Enviroment.ContentRootPath;

            #endregion

            services.AddIISServerConfigSetUp();

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISMSService, SMSService>();
            services.AddScoped<AuthenFilterAttribute>();


            services.AddIdentityConfigSetUp();

            services.AddIdentityServer4SetUp();

            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddRazorPages();
            services.AddMvc().AddSessionStateTempDataProvider();

            services.AddMemoryCache();          

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(3);
            });

            services.AddLoggingSetUp();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SCSS.IdentityServer4", Version = "v1" });
            });

            string accountSid = AppSettingValues.TwilioAccountSID;
            string authToken = AppSettingValues.TwilioAuthToken;

            TwilioClient.Init(accountSid, authToken);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SCSS.IdentityServer4 v1"));
            }
            app.UseDeveloperExceptionPage();


            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict, Secure = CookieSecurePolicy.None });

            

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseExceptionHandlerSetUp();

            app.UseInitializeDatabaseSetUp();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
