using System.Text.Json.Serialization;

namespace TempStation.Core.ExternalDataProviders.ForecastProviders.OpenWeatherMap.Models
{
    public class Coord
    {
        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }
    }
}