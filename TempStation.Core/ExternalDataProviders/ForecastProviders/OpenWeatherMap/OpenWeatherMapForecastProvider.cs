using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TempStation.Core.Constants;
using TempStation.Core.ExternalDataProviders.ForecastProviders.Contracts;
using TempStation.Core.ExternalDataProviders.ForecastProviders.Models;
using TempStation.Core.ExternalDataProviders.ForecastProviders.OpenWeatherMap.Models;

namespace TempStation.ExternalDataProviders.ForecastProviders.OpenWeatherMap
{
    public class OpenWeatherMapForecastProvider : IForecastProvider
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _cityId;
        private readonly string _apiKey;

        private ForecastData lastForecastData;
        private DateTime dataReceivedAt;

        public OpenWeatherMapForecastProvider(
            ILogger<OpenWeatherMapForecastProvider> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _apiKey = configuration[Constants.OpenWeatherMapConfigApiKey];
            _cityId = configuration[Constants.OpenWeatherMapConfigCityId];

            if (_apiKey == null || _cityId == null)
            {
                throw new ArgumentException("OpenWeatherMap: apikey and cityid are mandatory.");
            }
        }

        public async Task<ForecastData> GetCurrentForecastAsync()
        {
            if ((DateTime.Now - dataReceivedAt).TotalSeconds < (60 * 60))
            {
                return lastForecastData;
            }

            var client = _httpClientFactory.CreateClient(Constants.OpenWeatherMapHttpClientName);
            var response = await client.GetAsync($"weather?id={_cityId}&appid={_apiKey}&units=metric");
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                string data = await content.ReadAsStringAsync();
                _logger.LogDebug(data);
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(data);
                lastForecastData = weatherData.Convert();
                dataReceivedAt = DateTime.Now;
                return lastForecastData;
            }

            _logger.LogError("GetCurrentForecastAsync IsSuccessStatusCode = false");
            return null;
        }
    }
}
