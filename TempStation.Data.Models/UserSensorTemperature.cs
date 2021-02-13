using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TempStation.Data.Models
{
    public class UserSensorTemperature : SensorTemperature
    {
        public UserSensorTemperature() : base()
        {
            
        }
        
        public string UserSensorId { get; set; }
    }
}
