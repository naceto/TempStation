using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TempStation.Data.Models;
using TempStation.Models.ChartsJs;
using TempStation.Models.ViewModels;
using TempStation.Models.ViewModels.Sensors;
using TempStation.Services.Data.Contracts;

namespace TempStation.Controllers
{
    [Authorize]
    public class SensorsController : Controller
    {
        private const string TimeFormat = "H:mm";

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

            var listSensorData = new List<UserSensorsViewModel>();
            var sensors = _userSensorsService.AllByUserId(user.Id);
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

        [Authorize]
        public async Task<IActionResult> ViewSensorData(string id)
        {
            _logger.LogInformation($"{nameof(SensorsController.ViewSensorData)} called.");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tempChartData = new ChartData<double>
            {
                Datasets = new List<ChartDataset<double>>
                {
                    new ChartDataset<double>
                    {
                        Label = "Temperature",
                        YaxisID = "y-axis-1",
                        BorderColor = "rgb(255, 99, 132)",
                        BackgroundColor = "rgb(255, 99, 132)"
                    }
                }
            };

            var humiChartData = new ChartData<double>
            {
                Datasets = new List<ChartDataset<double>>
                {
                    new ChartDataset<double>
                    {
                        Label = "Humidity",
                        YaxisID = "y-axis-2",
                        BorderColor = "rgb(54, 162, 235)",
                        BackgroundColor = "rgb(54, 162, 235)"
                    }
                }
            };

            var sensorTemperatureData = await _temperatureService
                .GetByTimeIntervalGroupedByHour(DateTime.UtcNow.AddDays(-24), sensorId: id)
                .ToListAsync();

            var labels = new List<string>();
            foreach (var temp in sensorTemperatureData)
            {
                labels.Add(temp.DateTime.ToString(TimeFormat));
                tempChartData.Datasets[0].Data.Add(Math.Round(temp.Temperature, 1));
                humiChartData.Datasets[0].Data.Add(Math.Round(temp.Humidity, 1));
            }

            tempChartData.Labels = labels;
            humiChartData.Labels = labels;

            var chartViewData = new ChartsViewModel
            {
                TemperatureChartData = tempChartData,
                HumidityChartData = humiChartData
            };

            return View(chartViewData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogInformation($"{nameof(HomeController.Error)} called.");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
