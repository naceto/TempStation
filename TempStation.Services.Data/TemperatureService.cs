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
        private readonly IRepository<SensorTemperature> _temperatures;

        public TemperatureService(IRepository<SensorTemperature> temperatures)
        {
            _temperatures = temperatures;
        }

        public async Task<int> Add(SensorTemperature temperature)
        {
            this._temperatures.Add(temperature);
            int result = await this._temperatures.SaveChangesAsync();
            return result;
        }

        public IQueryable<SensorTemperature> All()
        {
            var all = this._temperatures.All();
            return all;
        }

        public IQueryable<SensorTemperature> GetByTimeIntervalGroupedByHour(DateTime from, DateTime? To = null)
        {
            var groupedTemperature = this._temperatures
                .All()
                .Where(t => t.DateTime >= from && (!To.HasValue || t.DateTime <= To))
                .OrderByDescending(t => t.Id)
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
            var latest = await this._temperatures.All()
                    .OrderByDescending(t => t.Id)
                    .FirstOrDefaultAsync();

            return latest;
        }
    }
}
