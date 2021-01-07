using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks; 

namespace TempStation.Hubs
{
    public class TemperatureHub : Hub
    {
        public static readonly string ForecastHubEndpoint = "ReceiveForecast";
        public async Task SendForecast(string json, string json2)
        {
            await Clients.All.SendAsync(ForecastHubEndpoint, json, json2).ConfigureAwait(false);
        }
    }
}
