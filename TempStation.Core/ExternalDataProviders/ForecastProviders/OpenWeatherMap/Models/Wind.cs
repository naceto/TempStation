using System.Text.Json.Serialization;

namespace TempStation.Core.ExternalDataProviders.ForecastProviders.OpenWeatherMap.Models
{
    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Deg { get; set; }
    }
}