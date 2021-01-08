using Newtonsoft.Json;

namespace TempStation.Core.ExternalDataProviders.ForecastProviders.OpenWeatherMap.Models
{
    public class Clouds
    {
        [JsonProperty("all")]
        public int All { get; set; }
    }
}