using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TempStation.Models;
using Iot.Device.DHTxx;
using Microsoft.EntityFrameworkCore;
using TempStation.Data;
using TempStation.Data.Models;
using TempStation.Classes.Charts;

namespace TempStation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Dht11 _dht11;
        private readonly TemperatureDbContext _dbContext; 

        public HomeController(ILogger<HomeController> logger, Dht11 dht11, TemperatureDbContext dbContext)
        {
            _logger = logger;
            _dht11 = dht11;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Temperature()
        {

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

            var labels = new List<string>();
            var tempDbData = _dbContext
                    .Temperatures
                    .OrderByDescending(t => t.Id)
                    .Where(t => t.DateTime >= DateTime.UtcNow.AddHours(-12))
                    .OrderBy(t => t.Id);

            foreach (var temp in tempDbData) 
            {
                labels.Add(temp.DateTime.ToString("hh:mm"));
                tempChartData.Datasets[0].Data.Add(temp.Temperature);
                humiChartData.Datasets[0].Data.Add(temp.Humidity);
            }

            tempChartData.Labels = labels;
            humiChartData.Labels = labels;

            var chartViewData = new TemperatureAndHumidityChartViewModel<double>
            {
                TemperatureChartData = tempChartData,
                HumidityChartData = humiChartData
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
