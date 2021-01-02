using System;
using TempStation.Classes.Charts;
using Newtonsoft.Json;

namespace TempStation.Models
{
    public class TemperatureAndHumidityChartViewModel<T>
    {
        [JsonProperty("temperatureChartData")]
        public ChartData<T> TemperatureChartData { get; set; }

        [JsonProperty("humidityChartData")]
        public ChartData<T> HumidityChartData { get; set; }
    }
}
