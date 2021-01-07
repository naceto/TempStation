using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TempStation.Classes.Charts;
using TempStation.Core.ExternalDataProviders.ForecastProviders.Contracts;
using TempStation.Models;
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

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"{nameof(HomeController.Index)} called.");
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

            var tempDbData = await _temperatureService
                    .All()
                    .Where(t => t.DateTime >= DateTime.UtcNow.AddHours(-24))
                    .OrderByDescending(t => t.Id)
                    .GroupBy(t => new { t.DateTime.Date, t.DateTime.Hour })
                    .Select(g => new
                    {
                        DateTime = new DateTime(g.Key.Date.Year, g.Key.Date.Month, g.Key.Date.Day, g.Key.Hour, 0, 0).ToLocalTime(),
                        Temperature = g.Average(gt => gt.Temperature),
                        Humidity = g.Average(gt => gt.Humidity)
                    }).ToListAsync();

            var labels = new List<string>();
            foreach (var temp in tempDbData)
            {
                labels.Add(temp.DateTime.ToString(TimeFormat));
                tempChartData.Datasets[0].Data.Add(Math.Round(temp.Temperature, 1));
                humiChartData.Datasets[0].Data.Add(Math.Round(temp.Humidity, 1));
            }

            tempChartData.Labels = labels;
            humiChartData.Labels = labels;

            var chartViewData = new IndexViewModel
            {
                TemperatureChartData = tempChartData,
                HumidityChartData = humiChartData
            };

            var latestTemperature = await _temperatureService.GetLatest();
            if (latestTemperature != null)
            {
                chartViewData.SensorTemperature = new SensorTemperatureModel(
                    latestTemperature.Temperature,
                    latestTemperature.Humidity,
                    latestTemperature.DateTime);
            }

            var currentForecastData = await _forecastProvider.GetCurrentForecastAsync();
            chartViewData.ForecastTemperature = new ForecastTemperatureModel
            {
                Temperature = currentForecastData.Temperature,
                Icon = currentForecastData.Icon
            };

            return View(chartViewData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
