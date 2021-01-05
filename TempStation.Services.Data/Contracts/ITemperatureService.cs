using System.Linq;
using System.Threading.Tasks;
using TempStation.Data.Models;

namespace TempStation.Services.Data.Contracts
{
    public interface ITemperatureService
    {
        IQueryable<TemperatureData> All();

        Task<TemperatureData> GetLatest();

        Task<int> Add(TemperatureData temperature);
    }
}
