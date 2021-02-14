using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            ITemperatureService temperatureService,
            IUserSensorsService userSensorService)
        {
            _logger = logger;
            _temperatureService = temperatureService;
            _userSensorService = userSensorService;
        }

        [HttpGet]
        public IActionResult Ping()
        {
            _logger.LogInformation($"{nameof(SensorDataController.Ping)} called.");
            return Ok("Api is working!");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SensorDataPostModel requestSensorData)
        {
            _logger.LogInformation($"{nameof(SensorDataController.Post)} called.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userSensor = _userSensorService.GetByMacAddress(requestSensorData.MacAddress);

            if (userSensor == null)
            {
                return BadRequest($"Sensor with MAC: {requestSensorData.MacAddress} does not exist.");
            }

            await _temperatureService.AddAsync(new SensorTemperature
            {
                Humidity = requestSensorData.Humidity,
                Temperature = requestSensorData.Temperature,
                UserSensorId = userSensor.Id
            });

            return Ok();
        }
    }
}
