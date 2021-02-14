using Newtonsoft.Json;
using TempStation.Models.ChartsJs;

namespace TempStation.Models.ViewModels
{
    public class ChartsViewModel
    {
        [JsonProperty("temperatureChartData")]
        public ChartData<double> TemperatureChartData { get; set; }

        [JsonProperty("humidityChartData")]
        public ChartData<double> HumidityChartData { get; set; }
    }
}
