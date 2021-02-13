using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TempStation.Data.Models;

namespace TempStation.Data
{
    public interface ITemperatureDbContext
    {
        DbSet<SensorTemperature> MainSensorTemperatures { get; set; }

        DbSet<UserSensor> UserSensors { get; set; }

        DbSet<UserSensorTemperature> UserSensorTemperatures { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();

        void Dispose();
    }
}