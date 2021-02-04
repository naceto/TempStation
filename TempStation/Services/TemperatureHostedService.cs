using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TempStation.Core.ExternalDataProviders.ForecastProviders.Contracts;
using TempStation.Hubs;
using TempStation.Services.Data.Contracts;
using TempStation.Models;

namespace TempStation.Services
{
    public class TemperatureHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IForecastProvider _forecastProvider;
        private readonly IHubContext<TemperatureHub> _forecastHubContext;
        private readonly IServiceProvider _serviceProvider;

        private Timer _timer;

        public TemperatureHostedService(
            ILogger<TemperatureHostedService> logger,
            IForecastProvider forecastProvider,
            IHubContext<TemperatureHub> forecastHubContext,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _forecastProvider = forecastProvider;
            _forecastHubContext = forecastHubContext;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(nameof(TemperatureHostedService) + " is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation(nameof(TemperatureHostedService) + " is working.");

            using var scope = _serviceProvider.CreateScope();
            var temperatureService = scope.ServiceProvider.GetService<ITemperatureService>();
            var latestTemperature = await temperatureService.GetLatest();

            SensorTemperatureModel sensorTemperatureModel = null;
            if (latestTemperature != null) {
                sensorTemperatureModel = new SensorTemperatureModel(
                    latestTemperature.Temperature,
                    latestTemperature.Humidity,
                    latestTemperature.DateTime);
            }
    
            var currentForecastData = await _forecastProvider.GetCurrentForecastAsync();
            var forecastTemperatureModel = new ForecastTemperatureModel
            {
                Temperature = currentForecastData.Temperature,
                Icon = currentForecastData.Icon,
                MinTemperature = currentForecastData.MinTemperature,
                MaxTemperature = currentForecastData.MaxTemperature,
                TakenAtTime = currentForecastData.TakenAtTime.ToString("HH:mm:ss")
            };

            await _forecastHubContext.Clients.All.SendAsync(
                TemperatureHub.ForecastHubEndpoint,
                forecastTemperatureModel, sensorTemperatureModel
                ).ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(nameof(TemperatureHostedService) + " is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
