using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Repaso_Net.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DinkToPdf.Contracts;
using DinkToPdf;
using Repaso_Net.Models;

namespace Repaso_Net
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
             
             var connectionString = Configuration.GetConnectionString("DefaultConnection");

             services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                     connectionString , builder => {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    }));
            services.AddDatabaseDeveloperPageExceptionFilter();

        
            services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

             services.AddSingleton(typeof(IConverter),
             new SynchronizedConverter(new PdfTools()));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
