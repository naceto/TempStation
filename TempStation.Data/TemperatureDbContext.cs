using Microsoft.EntityFrameworkCore;
using TempStation.Data.Models;

namespace TempStation.Data 
{
    public class TemperatureDbContext : DbContext, ITemperatureDbContext
    {
        public DbSet<SensorTemperature> Temperatures { get; set; }

        public TemperatureDbContext(DbContextOptions<TemperatureDbContext> options) 
            : base(options)
        { }
    }
}
