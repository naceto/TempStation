using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TempStation.Models
{
    public class TemperatureDbModel
    {
        public TemperatureDbModel() 
        {
            DateTime = DateTime.UtcNow;
        }

        public long Id { get;  set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public DateTime DateTime {get; set; }

        [NotMapped]
        public bool IsLastReadSuccessful { get; set; }
    }
}
