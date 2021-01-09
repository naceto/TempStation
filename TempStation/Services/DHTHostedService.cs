using Iot.Device.DHTxx;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TempStation.Data.Models;
using TempStation.Services.Data.Contracts;

namespace TempStation.Services
{
    internal class DHTHostedService : IHostedService, IDisposable
    {
        private readonly Dht11 _dht;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        private Timer _timer;

        public DHTHostedService(
            ILogger<DHTHostedService> logger,
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
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("DHT Service is working.");
            var temperature = new SensorTemperatureData
            {
                Temperature = _dht.Temperature.Value,
                Humidity = _dht.Humidity.Value,
                IsLastReadSuccessful = _dht.IsLastReadSuccessful
            };

            if (temperature.IsLastReadSuccessful)
            {
                _logger.LogInformation("DHT LastReadSuccessful.");
                using var scope = _serviceProvider.CreateScope();
                var temperatureService = scope.ServiceProvider.GetService<ITemperatureService>();
                var latestTemperature = await temperatureService.Add(temperature);
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
