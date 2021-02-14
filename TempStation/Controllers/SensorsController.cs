using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TempStation.Data.Models;
using TempStation.Models.ViewModels;
using TempStation.Models.ViewModels.Sensors;
using TempStation.Services.Data.Contracts;

namespace TempStation.Controllers
{
    [Authorize]
    public class SensorsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<TempStationUser> _userManager;
        private readonly IUserSensorsService _userSensorsService;
        private readonly ITemperatureService _temperatureService;

        public SensorsController(
            ILogger<HomeController> logger,
            UserManager<TempStationUser> userManager, 
            IUserSensorsService userSensorsService,
            ITemperatureService temperatureService)
        {
            _logger = logger;
            _userManager = userManager;
            _userSensorsService = userSensorsService;
            _temperatureService = temperatureService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"{nameof(SensorsController.Index)} called.");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var sensors = _userSensorsService.AllByUserId(user.Id);

            var listSensorData = new List<UserSensorsViewModel>();
            foreach (var sensor in sensors)
            {
                listSensorData.Add(new UserSensorsViewModel
                {
                    Id = sensor.Id,
                    Name = sensor.Name,
                    MacAddress = sensor.MacAddress
                });
            }

            return View(listSensorData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogInformation($"{nameof(HomeController.Error)} called.");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
