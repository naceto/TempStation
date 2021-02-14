using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TempStation.Data.Models;
using TempStation.Data.Repositories;
using TempStation.Services.Data.Contracts;

namespace TempStation.Services.Data
{
    public class TemperatureService : ITemperatureService
    {
        private readonly IRepository<SensorTemperature> _sensorTemperatures;
        private readonly IRepository<TempStationUser> _tempStationUsers;
        private readonly IRepository<UserSensor> _userSensors;

        public TemperatureService(
            IRepository<SensorTemperature> mainSensorTemperatures,
            IRepository<TempStationUser> tempStationUsers,
            IRepository<UserSensor> userSensors)
        {
            _sensorTemperatures = mainSensorTemperatures;
            _tempStationUsers = tempStationUsers;
            _userSensors = userSensors;
        }

        public async Task<int> AddAsync(SensorTemperature temperature)
        {
            _sensorTemperatures.Add(temperature);
            int result = await _sensorTemperatures.SaveChangesAsync();
            return result;
        }

        public IQueryable<SensorTemperature> All()
        {
            var all = _sensorTemperatures.All();
            return all;
        }

        public IQueryable<SensorTemperature> GetByTimeIntervalGroupedByHour(DateTime from, DateTime? To = null)
        {
            var groupedTemperature = _sensorTemperatures
                .All()
                .Where(t => t.UserSensor == null && t.DateTime >= from && (!To.HasValue || t.DateTime <= To))
                .OrderByDescending(t => t.DateTime)
                .GroupBy(t => new { t.DateTime.Date, t.DateTime.Hour })
                .Select(g => new SensorTemperature
                {
                    DateTime = new DateTime(g.Key.Date.Year, g.Key.Date.Month, g.Key.Date.Day, g.Key.Hour, 0, 0).ToLocalTime(),
                    Temperature = g.Average(gt => gt.Temperature),
                    Humidity = g.Average(gt => gt.Humidity)
                });

            return groupedTemperature;
        }

        public IQueryable<SensorTemperature> GetBySensorIdAndByStartTimeGroupedByHour(string sensorId, DateTime from)
        {
            var groupedTemperature = _sensorTemperatures
                .All()
                .Include(t => t.UserSensor)
                .Where(t => t.UserSensor != null && t.UserSensor.Id == sensorId && t.DateTime >= from)
                .OrderByDescending(t => t.DateTime)
                .GroupBy(t => new { t.DateTime.Date, t.DateTime.Hour })
                .Select(g => new SensorTemperature
                {
                    DateTime = new DateTime(g.Key.Date.Year, g.Key.Date.Month, g.Key.Date.Day, g.Key.Hour, 0, 0).ToLocalTime(),
                    Temperature = g.Average(gt => gt.Temperature),
                    Humidity = g.Average(gt => gt.Humidity)
                });

            return groupedTemperature;
        }

        public async Task<SensorTemperature> GetLatest()
        {
            var latest = await _sensorTemperatures.All()
                .Where(t => t.UserSensor == null)
                .OrderByDescending(t => t.DateTime)
                .FirstOrDefaultAsync();

            return latest;
        }
    }
}
