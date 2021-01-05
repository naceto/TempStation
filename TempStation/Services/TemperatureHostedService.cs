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
                TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation(nameof(TemperatureHostedService) + " is working.");
            
            var forecastData = await _forecastProvider.GetCurrentForecastAsync();

            using var scope = _serviceProvider.CreateScope();
            var temperatureService = scope.ServiceProvider.GetService<ITemperatureService>();
            var latestTemperature = await temperatureService.GetLatest();

            await _forecastHubContext.Clients.All.SendAsync(
                TemperatureHub.ForecastHubEndpoint, 
                forecastData, latestTemperature
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
