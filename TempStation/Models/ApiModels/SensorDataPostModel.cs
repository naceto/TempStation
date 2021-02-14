using System.ComponentModel.DataAnnotations;

namespace TempStation.Models.ApiModels
{
    public class SensorDataPostModel
    {
        public string SensorId { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        [Required]
        public string MacAddress { get; set; }
    }
}
