using System;
using System.Linq;
using System.Threading.Tasks;
using TempStation.Data.Models;

namespace TempStation.Services.Data.Contracts
{
    public interface ITemperatureService
    {
        IQueryable<SensorTemperatureData> All();

        IQueryable<SensorTemperatureData> GetByTimeIntervalGroupedByHour(DateTime from, DateTime? To = null);

        Task<SensorTemperatureData> GetLatest();

        Task<int> Add(SensorTemperatureData temperature);
    }
}
