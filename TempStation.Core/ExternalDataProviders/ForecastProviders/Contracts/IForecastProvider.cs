using System.Threading.Tasks;
using TempStation.Core.ExternalDataProviders.ForecastProviders.Models;

namespace TempStation.Core.ExternalDataProviders.ForecastProviders.Contracts
{
    public interface IForecastProvider
    {
        Task<ForecastData> GetCurrentForecastAsync();
    }
}
