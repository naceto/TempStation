using System;

namespace TempStation.Core.ExternalDataProviders.ForecastProviders.Models
{
    public class ForecastData
    {
        public double Temperature { get; set; }

        public double FeelsLikeTemperature { get; set; }

        public double MinTemperature { get; set; }

        public double MaxTemperature { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public double WindSpeed { get; set; }

        public string City { get; set; }

        public string Icon { get; set; }

        public DateTime TakenAtTime { get; set; }
    }
}
