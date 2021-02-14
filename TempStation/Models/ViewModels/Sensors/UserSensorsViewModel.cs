using TempStation.Models.ChartsJs;

namespace TempStation.Models.ViewModels.Sensors
{
    public class UserSensorsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string MacAddress { get; set; }

        public ChartData<double> TemperatureChartData { get; set; }

        public ChartData<double> HumidityChartData { get; set; }
    }
}
