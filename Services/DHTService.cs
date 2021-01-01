using Iot.Device.DHTxx;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TempStation.Models;
using TempStation.Data;

namespace TempStation.Services
{
    internal class DHTService : IHostedService, IDisposable
    {
        private readonly Dht11 _dht;
        private readonly ILogger _logger;
        private Timer _timer;
        private IServiceProvider _serviceProvider;

        public DHTService(
            ILogger<DHTService> logger,
            Dht11 dhtController,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _dht = dhtController;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DHT Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("DHT Service is working.");
            var temperature = new TemperatureDbModel 
            {
                Temperature = _dht.Temperature.Value,
                Humidity = _dht.Humidity.Value,
                IsLastReadSuccessful = _dht.IsLastReadSuccessful
            };

            if (temperature.IsLastReadSuccessful)
            {
                _logger.LogInformation("DHT LastReadSuccessful.");
                // save to DB
                using (var scope = _serviceProvider.CreateScope()) 
                {
                    var dbContext = scope.ServiceProvider.GetService<TemperatureDbContext>();
                    dbContext.Add<TemperatureDbModel>(temperature);
                    dbContext.SaveChanges();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DHT Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
