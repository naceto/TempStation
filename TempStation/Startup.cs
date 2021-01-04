using Iot.Device.DHTxx;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TempStation.Data;
using TempStation.Data.Models;
using TempStation.Data.Repositories;
using TempStation.Services;
using TempStation.Services.Data;
using TempStation.Services.Data.Contracts;

namespace TempStation
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
            services.AddDbContext<TemperatureDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), 
                b => b.MigrationsAssembly("TempStation.Data")));

            services.AddSingleton(new Dht11(14));
            services.AddHostedService<DHTService>();
            services.AddControllersWithViews();
            services.AddTransient<ITemperatureDbContext, TemperatureDbContext>();
            services.AddTransient<IRepository<TemperatureData>, GenericRepository<TemperatureData>>();
            services.AddTransient<ITemperatureService, TemperatureService>();
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

            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
