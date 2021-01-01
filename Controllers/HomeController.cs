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
            var tempDbData = _dbContext.Temperatures.OrderByDescending(t => t.Id).Take(50);
            var viewTempData = new List<TemperatureViewModel>();
            foreach (var temp in tempDbData) 
            {
                viewTempData.Add(new TemperatureViewModel
                {
                    Temperature = temp.Temperature,
                    Humidity = temp.Humidity,
                    DateTime = temp.DateTime
                });
            }

            return View(viewTempData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
