namespace TempStation.Services.Data
{
    using System.Linq;
    using TempStation.Data.Models;
    using TempStation.Data.Repositories;
    using TempStation.Services.Data.Contracts;

    public class TemperatureService : ITemperatureService
    {
        private readonly IRepository<TemperatureData> _temperatures;

        public TemperatureService(IRepository<TemperatureData> temperatures)
        {
            _temperatures = temperatures;
        }

        public IQueryable<TemperatureData> All()
        {
            var all = this._temperatures.All();
            return all;
        }
    }
}
