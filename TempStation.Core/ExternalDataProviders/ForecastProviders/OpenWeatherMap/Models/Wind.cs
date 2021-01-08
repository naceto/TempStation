using Newtonsoft.Json;

namespace TempStation.Core.ExternalDataProviders.ForecastProviders.OpenWeatherMap.Models
{
    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public int Deg { get; set; }
    }
}