using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks; 

namespace TempStation.Hubs
{
    public class TemperatureHub : Hub
    {
        public static readonly string ForecastHubEndpoint = "ReceiveForecast";
        public async Task SendForecast(string json)
        {
            await Clients.All.SendAsync(ForecastHubEndpoint, json).ConfigureAwait(false);
        }
    }
}
