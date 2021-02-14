using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TempStation.Core.ExternalDataProviders.ForecastProviders.Contracts;
using TempStation.Models;
using TempStation.Models.ChartsJs;
using TempStation.Models.ViewModels;
using TempStation.Services.Data.Contracts;

namespace TempStation.Controllers
{
    public class HomeController : Controller
    {
        private const string TimeFormat = "H:mm";

        private readonly ILogger<HomeController> _logger;
        private readonly ITemperatureService _temperatureService;
        private readonly IForecastProvider _forecastProvider;

        public HomeController(
            ILogger<HomeController> logger,
            ITemperatureService temperatureService,
            IForecastProvider forecastProvider)
        {
            _logger = logger;
            _temperatureService = temperatureService;
            _forecastProvider = forecastProvider;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"{nameof(HomeController.Index)} called.");

            var indexViewModel = new IndexViewModel();

            // latest sensor data
            var latestTemperature = await _temperatureService.GetLatest();
            if (latestTemperature != null)
            {
                indexViewModel.SensorTemperature = new SensorTemperatureResponseModel(
                    latestTemperature.Temperature,
                    latestTemperature.Humidity,
                    latestTemperature.DateTime);
            }

            // forecast data
            var currentForecastData = await _forecastProvider.GetCurrentForecastAsync();
            indexViewModel.ForecastTemperature = new ForecastTemperatureResponseModel
            {
                Temperature = currentForecastData.Temperature,
                Icon = currentForecastData.Icon,
                MinTemperature = currentForecastData.MinTemperature,
                MaxTemperature = currentForecastData.MaxTemperature,
                TakenAtTime = currentForecastData.TakenAtTime.ToString("HH:mm:ss")
            };

            return View(indexViewModel);
        }

        public async Task<IActionResult> Charts() 
        {
            _logger.LogInformation($"{nameof(HomeController.Charts)} called.");

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

            // sensor chart data
            var tempDbData = await _temperatureService
                    .GetByTimeIntervalGroupedByHour(DateTime.UtcNow.AddHours(-24))
                    .ToListAsync();

            var labels = new List<string>();
            foreach (var temp in tempDbData)
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
