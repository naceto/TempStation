using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TempStation.Data.Models
{
    public class SensorTemperature
    {
        public SensorTemperature()
        {
            DateTime = DateTime.UtcNow;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public DateTime DateTime { get; set; }

        public virtual UserSensor UserSensor { get; set; }

        [NotMapped]
        public bool IsLastReadSuccessful { get; set; }
    }
}
