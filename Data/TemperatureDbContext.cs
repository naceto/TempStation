using System;
using Microsoft.EntityFrameworkCore;
using TempStation.Models;

namespace TempStation.Data 
{
    public class TemperatureDbContext : DbContext
    {
        public DbSet<TemperatureDbModel> Temperatures { get; set; }
        public TemperatureDbContext(DbContextOptions<TemperatureDbContext> options) 
            : base(options)
        { }
    }
}
