using System;

namespace TempStation.Models
{
    public class ForecastTemperatureModel
    {
        private double temperature;

        public double Temperature { get => temperature; set => temperature = Math.Round(value); }

        public string Icon { get; set; }
    }
}
