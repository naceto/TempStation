using System;

namespace TempStation.Models
{
    public class ForecastTemperatureModel
    {
        private double temperature;
        private double maxTemperature;
        private double minTemperature;

        public double Temperature { get => temperature; set => temperature = Math.Round(value); }

        public double MaxTemperature { get => maxTemperature; set => maxTemperature = Math.Round(value); }

        public double MinTemperature { get => minTemperature; set => minTemperature = Math.Round(value); }

        public string Icon { get; set; }

        public string TakenAtTime { get; set; }
    }
}
