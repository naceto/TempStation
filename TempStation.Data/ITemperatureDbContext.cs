using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TempStation.Data.Models;

namespace TempStation.Data
{
    public interface ITemperatureDbContext
    {
        DbSet<SensorTemperatureData> Temperatures { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();

        void Dispose();
    }
}