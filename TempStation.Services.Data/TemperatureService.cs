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
        private readonly IRepository<SensorTemperature> _mainSensorTemperatures;

        public TemperatureService(IRepository<SensorTemperature> mainSensorTemperatures)
        {
            _mainSensorTemperatures = mainSensorTemperatures;
        }

        public async Task<int> AddAsync(SensorTemperature temperature)
        {
            this._mainSensorTemperatures.Add(temperature);
            int result = await _mainSensorTemperatures.SaveChangesAsync();
            return result;
        }

        public IQueryable<SensorTemperature> All()
        {
            var all = _mainSensorTemperatures.All();
            return all;
        }

        public IQueryable<SensorTemperature> GetByTimeIntervalGroupedByHour(DateTime from, DateTime? To = null)
        {
            var groupedTemperature = _mainSensorTemperatures
                .All()
                .Where(t => t.UserSensorId == null && t.DateTime >= from && (!To.HasValue || t.DateTime <= To))
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
            var latest = await _mainSensorTemperatures.All()
                .Where(t => t.UserSensorId == null)
                .OrderByDescending(t => t.DateTime)
                .FirstOrDefaultAsync();

            return latest;
        }
    }
}
