using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using TempStation.Data.Models;
using TempStation.Models.ApiModels;
using TempStation.Services.Data.Contracts;

namespace TempStation.Controllers
{
    public class SensorDataController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITemperatureService _temperatureService;
        private readonly IUserSensorsService _userSensorService;

        public SensorDataController(
            ILogger<HomeController> logger,
            ITemperatureService temperatureService)
        {
            _logger = logger;
            _temperatureService = temperatureService;
        }

        [HttpGet]
        public IActionResult Ping()
        {
            _logger.LogInformation($"{nameof(SensorDataController.Ping)} called.");
            return Ok("Api is working!");
        }

        [HttpPost]
        public async Task<IActionResult> Post(SensorDataPostModel sensorData)
        {
            _logger.LogInformation($"{nameof(SensorDataController.Post)} called.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userSensor = _userSensorService
                .AllByUserId(sensorData.UserId)
                .Where(us => us.MacAddress == sensorData.MacAddress)
                .FirstOrDefault();

            if (userSensor == null)
            {
                return BadRequest($"Sensor for user Id: {sensorData.UserId} with MAC address: {sensorData.MacAddress} does not exists.");
            }

            await _temperatureService.AddAsync(new SensorTemperature
            {
                Humidity = sensorData.Humidity,
                Temperature = sensorData.Temperature,
                UserSensorId = userSensor.Id
            });

            return Ok();
        }
    }
}
