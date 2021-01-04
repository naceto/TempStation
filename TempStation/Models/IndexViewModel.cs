using System;
using TempStation.Classes.Charts;
using Newtonsoft.Json;

namespace TempStation.Models
{
    public class IndexViewModel
    {
        public IndexViewModel() 
        {
            CurrentTemperature = "N/A";
            CurrentHumidity = "N/A";
            TakenAtTime = "N/A";
        }

        [JsonProperty("temperatureChartData")]
        public ChartData<double> TemperatureChartData { get; set; }

        [JsonProperty("humidityChartData")]
        public ChartData<double> HumidityChartData { get; set; }

        [JsonIgnore]
        public string CurrentTemperature { get; set; }

        [JsonIgnore]
        public string CurrentHumidity { get; set; }

        [JsonIgnore]
        public string TakenAtTime { get; set; }
    }
}
