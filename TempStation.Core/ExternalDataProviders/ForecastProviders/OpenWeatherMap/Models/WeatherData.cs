using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using TempStation.Core.ExternalDataProviders.ForecastProviders.Models;
using TempStation.Core.Generic;

namespace TempStation.Core.ExternalDataProviders.ForecastProviders.OpenWeatherMap.Models
{
    public class WeatherData : IConvert<ForecastData>
    {
        [JsonPropertyName("coord")]
        public Coord Coord { get; set; }

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("main")]
        public Main Main { get; set; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; }

        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; }

        [JsonPropertyName("dt")]
        public int Dt { get; set; }

        [JsonPropertyName("sys")]
        public Sys Sys { get; set; }

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("cod")]
        public int Cod { get; set; }

        public ForecastData Convert()
        {
            return new ForecastData
            {
                Temperature = this.Main.Temp,
                FeelsLikeTemperature = this.Main.FeelsLike,
                MaxTemperature = this.Main.TempMax,
                MinTemperature = this.Main.TempMin,
                Pressure = this.Main.Pressure,
                Humidity = this.Main.Humidity,
                WindSpeed = this.Wind.Speed,
                City = this.Name,
                Icon = this.Weather.FirstOrDefault()?.Icon,
                TakenAtTime = DateTime.Now
            };
        }
    }
}
