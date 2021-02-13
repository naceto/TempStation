using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TempStation.Models.ApiModels;
using TempStation.Services.Data.Contracts;

namespace TempStation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITemperatureService _temperatureService;

        public SensorDataController(
            ILogger<HomeController> logger,
            ITemperatureService temperatureService)
        {
            _logger = logger;
            _temperatureService = temperatureService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SensorDataPostModel sensorData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // check if mac adress is in the system
            // if yes add data
            // else return error

            return Ok();
        }
    }
}
