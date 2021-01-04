﻿using Iot.Device.DHTxx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TempStation.Classes.Charts;
using TempStation.Data;
using TempStation.Models;
using TempStation.Services.Data.Contracts;

namespace TempStation.Controllers
{
    public class HomeController : Controller
    {
        private const string TimeFormat = "H:mm";
        private readonly ILogger<HomeController> _logger;
        private readonly ITemperatureService _temperatureService;

        public HomeController(ILogger<HomeController> logger, ITemperatureService temperatureService)
        {
            _logger = logger;
            _temperatureService = temperatureService;
        }

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
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

            var tempDbData = _temperatureService
                    .All()
                    .OrderByDescending(t => t.Id)
                    .Where(t => t.DateTime >= DateTime.UtcNow.AddHours(-24))
                    .GroupBy(t => new { t.DateTime.Date, t.DateTime.Hour })
                    .Select(g => new
                    {
                        DateTime = new DateTime(g.Key.Date.Year, g.Key.Date.Month, g.Key.Date.Day, g.Key.Hour, 0, 0).ToLocalTime(),
                        Temperature = g.Average(gt => gt.Temperature),
                        Humidity = g.Average(gt => gt.Humidity)
                    });

            var labels = new List<string>();
            if (tempDbData != null) 
            {
                foreach (var temp in tempDbData)
                {
                    labels.Add(temp.DateTime.ToString(TimeFormat));
                    tempChartData.Datasets[0].Data.Add(Math.Round(temp.Temperature, 1));
                    humiChartData.Datasets[0].Data.Add(Math.Round(temp.Humidity, 1));
                }
            }

            tempChartData.Labels = labels;
            humiChartData.Labels = labels;

            var chartViewData = new IndexViewModel
            {
                TemperatureChartData = tempChartData,
                HumidityChartData = humiChartData
            };

            // latest temperature
            var latestTemperature = _temperatureService
                    .All()
                    .OrderByDescending(t => t.Id)
                    .FirstOrDefault();

            if (latestTemperature != null)
            {
                chartViewData.CurrentTemperature = Math.Round(latestTemperature.Temperature, 2).ToString();
                chartViewData.CurrentHumidity = Math.Round(latestTemperature.Humidity, 2).ToString();
                chartViewData.TakenAtTime = latestTemperature.DateTime.ToLocalTime().ToString(TimeFormat + ":ss");
            }

            return View(chartViewData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}