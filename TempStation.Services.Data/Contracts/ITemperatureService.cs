namespace TempStation.Services.Data.Contracts
{
    using System.Linq;
    using TempStation.Data.Models;

    public interface ITemperatureService
    {
        IQueryable<TemperatureData> All();
    }
}
