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
            var tempDbData = _dbContext.Temperatures.OrderBy(t => t.Id).Take(50);

            var viewChartData = new TemperatureChartViewModel<double>
            {
                Datasets = new List<TemperatureDataset<double>>
                {
                    new TemperatureDataset<double>
                    {
                        Label = "Temperature",
                        YaxisID = "y-axis-1",
                        BorderColor = "rgb(255, 99, 132)",
                        BackgroundColor = "rgb(255, 99, 132)"
                    },
                    new TemperatureDataset<double>
                    {
                        Label = "Humidity",
                        YaxisID = "y-axis-2",
                        BorderColor = "rgb(54, 162, 235)",
                        BackgroundColor = "rgb(54, 162, 235)"
                    }
                }
            };

            foreach (var temp in tempDbData) 
            {
                viewChartData.Labels.Add(temp.DateTime.ToString("hh:mm"));
                viewChartData.Datasets[0].Data.Add(temp.Temperature);
                viewChartData.Datasets[1].Data.Add(temp.Humidity);
            }

            return View(viewChartData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
