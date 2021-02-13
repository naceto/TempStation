using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TempStation.Data.Models;

namespace TempStation.Data 
{
    public class TemperatureDbContext : IdentityDbContext<TempStationUser>, ITemperatureDbContext
    {
        public DbSet<SensorTemperature> MainSensorTemperatures { get; set; }

        public DbSet<UserSensor> UserSensors { get; set; }

        public DbSet<UserSensorTemperature> UserSensorTemperatures { get; set; }

        public TemperatureDbContext(DbContextOptions<TemperatureDbContext> options) 
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
