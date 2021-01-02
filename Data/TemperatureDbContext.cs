using System;
using Microsoft.EntityFrameworkCore;
using TempStation.Data.Models;

namespace TempStation.Data 
{
    public class TemperatureDbContext : DbContext
    {
        public DbSet<TemperatureData> Temperatures { get; set; }
        public TemperatureDbContext(DbContextOptions<TemperatureDbContext> options) 
            : base(options)
        { }
    }
}
