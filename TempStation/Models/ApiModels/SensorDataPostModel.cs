using System.ComponentModel.DataAnnotations;

namespace TempStation.Models.ApiModels
{
    public class SensorDataPostModel
    {
        [Required]
        public double Temperature { get; set; }

        [Required]
        public double Humidity { get; set; }

        [Required]
        public string MacAddress { get; set; }
    }
}
