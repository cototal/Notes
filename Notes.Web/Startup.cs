using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cototal.AspNetCore.ApprovedEmailAccess.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Web.Config;
using Notes.Web.Services;

namespace Notes.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var googleAuthConfig = BindConfig<GoogleAuthConfig>("GoogleAuth");
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "Google";
            })
                .AddCookie("Cookies")
                .AddGoogle("Google", options =>
                {
                    options.ClientId = googleAuthConfig.ClientId;
                    options.ClientSecret = googleAuthConfig.ClientSecret;
                });
            services.AddSingleton<IVerifyAdminEmailOptions>(srv => new VerifyAdminEmailOptions(Configuration.GetValue<string>("AdminEmails")));
            services.AddSingleton<NoteService>(srv => new NoteService(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<AssetVersionFinder>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();
            app.UseMiddleware<VerifyAdminEmail>();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        private T BindConfig<T>(string sectionName)
            where T : new()
        {
            var config = new T();
            Configuration.GetSection(sectionName).Bind(config);
            return config;
        }
    }
}
