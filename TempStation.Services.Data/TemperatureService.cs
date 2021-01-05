using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TempStation.Data.Models;
using TempStation.Data.Repositories;
using TempStation.Services.Data.Contracts;

namespace TempStation.Services.Data
{
    public class TemperatureService : ITemperatureService
    {
        private readonly IRepository<TemperatureData> _temperatures;

        public TemperatureService(IRepository<TemperatureData> temperatures)
        {
            _temperatures = temperatures;
        }

        public async Task<int> Add(TemperatureData temperature)
        {
            this._temperatures.Add(temperature);
            int result = await this._temperatures.SaveChangesAsync();
            return result;
        }

        public IQueryable<TemperatureData> All()
        {
            var all = this._temperatures.All();
            return all;
        }

        public async Task<TemperatureData> GetLatest()
        {
            var latest = await this._temperatures.All()
                    .OrderByDescending(t => t.Id)
                    .FirstOrDefaultAsync();

            return latest;
        }
    }
}
