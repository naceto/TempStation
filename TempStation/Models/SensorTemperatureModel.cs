using System;

namespace TempStation.Models
{
    public class SensorTemperatureModel
    {
        public SensorTemperatureModel()
        {
            CurrentTemperature = "N/A";
            CurrentHumidity = "N/A";
            TakenAtTime = "N/A";
        }

        public SensorTemperatureModel(double temperature, double humidity, DateTime takenAtTime)
        {
            CurrentTemperature = Math.Round(temperature, 2).ToString();
            CurrentHumidity = Math.Round(humidity, 2).ToString();
            TakenAtTime = takenAtTime.ToLocalTime().ToString("HH:mm:ss");
        }

        public string CurrentTemperature { get; set; }

        public string CurrentHumidity { get; set; }

        public string TakenAtTime { get; set; }
    }
}
