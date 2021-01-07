using Newtonsoft.Json;
using TempStation.Classes.Charts;

namespace TempStation.Models
{
    public class IndexViewModel
    {
        [JsonProperty("temperatureChartData")]
        public ChartData<double> TemperatureChartData { get; set; }

        [JsonProperty("humidityChartData")]
        public ChartData<double> HumidityChartData { get; set; }

        [JsonIgnore]
        public SensorTemperatureModel SensorTemperature { get; set; }

        [JsonIgnore]
        public ForecastTemperatureModel ForecastTemperature { get; set; }
    }
}
