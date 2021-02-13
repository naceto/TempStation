namespace TempStation.Models.ApiModels
{
    public class SensorDataPostModel
    {
        public string Id { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public string MacAddress { get; set; }
    }
}
