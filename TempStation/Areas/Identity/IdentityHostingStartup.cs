using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TempStation.Areas.Identity.Data;

[assembly: HostingStartup(typeof(TempStation.Areas.Identity.IdentityHostingStartup))]
namespace TempStation.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<TempStationIdentityDbContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("TempStationIdentityDbContextConnection")));

                services.AddDefaultIdentity<TempStationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<TempStationIdentityDbContext>();
            });
        }
    }
}