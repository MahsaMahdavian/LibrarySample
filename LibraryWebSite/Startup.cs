using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LibraryWebSite.Data;
using LibraryWebSite.Data.Contract;
using LibraryWebSite.Data.Repositories;
using LibraryWebSite.IocConfig;
using LibraryWebSite.Services.Identity;
using LibraryWebSite.ViewModel.Setting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryWebSite
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
            services.Configure<SiteSetting>(Configuration.GetSection(nameof(SiteSetting)));
            services.AddDbContext<LibraryDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
            services.AddTransient<LibraryDBContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddControllersWithViews().AddNewtonsoftJson().AddRazorRuntimeCompilation();
            services.AddCustomIdentityServices();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login/Index";

            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminView", policy => policy.RequireRole("Admin","User"));
                options.AddPolicy("HappyBirthDay", policy => policy.RequireClaim(ClaimTypes.DateOfBirth, DateTime.Now.ToString("MM/dd")));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCustomIdentityServices();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
