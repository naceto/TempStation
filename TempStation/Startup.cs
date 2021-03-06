using Iot.Device.DHTxx;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TempStation.Core.Constants;
using TempStation.Core.ExternalDataProviders.ForecastProviders.Contracts;
using TempStation.Data;
using TempStation.Data.Models;
using TempStation.Data.Repositories;
using TempStation.ExternalDataProviders.ForecastProviders.OpenWeatherMap;
using TempStation.Hubs;
using TempStation.Services;
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
                options.UseSqlite(Configuration.GetConnectionString(Constants.DefaultConnectionStringConfigName),
                    b => b.MigrationsAssembly(Constants.EntityCoreMigrationAssembly)));

            services.AddDefaultIdentity<TempStationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<TemperatureDbContext>();

            services.AddSingleton(new Dht11(14));
            services.AddHostedService<DHTHostedService>();

            services.AddTransient<IRepository<TempStationUser>, GenericRepository<TempStationUser>>();
            services.AddTransient<IRepository<UserSensor>, GenericRepository<UserSensor>>();
            services.AddTransient<IRepository<SensorTemperature>, GenericRepository<SensorTemperature>>();

            services.AddTransient<ITemperatureService, Services.Data.TemperatureService>();
            services.AddTransient<IUserSensorsService, Services.Data.UserSensorsService>();


            services.AddHttpClient(Constants.OpenWeatherMapHttpClientName, c =>
            {
                c.BaseAddress = new Uri(Configuration[Constants.OpenWeatherMapConfigBaseUrl]);
            });

            services.AddSingleton<IForecastProvider, OpenWeatherMapForecastProvider>();
            services.AddHostedService<TemperatureHostedService>();

            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddRazorPages();
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

                endpoints.MapRazorPages();
                endpoints.MapHub<TemperatureHub>("/forecastHub");
            });
        }
    }
}
