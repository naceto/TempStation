using System.Text.Json.Serialization;

namespace TempStation.Core.ExternalDataProviders.ForecastProviders.OpenWeatherMap.Models
{
    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}