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
        private IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        private readonly string apiKey = "0ecba45f5fb8a23b22f2d7ab82d0f71b";
        private readonly string cityId = "727012";

        private ForecastData lastForecastData;
        private DateTime dataReceivedAt;


        public OpenWeatherMapForecastProvider(ILogger<OpenWeatherMapForecastProvider> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ForecastData> GetCurrentForecastAsync()
        {
            if ((DateTime.Now - dataReceivedAt).TotalSeconds < (60 * 60))
            {
                return lastForecastData;
            }

            var client = _httpClientFactory.CreateClient(Constants.OpenWeatherMapHttpClientName);
            var response = await client.GetAsync($"weather?id={cityId}&appid={apiKey}&units=metric");
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                string data = await content.ReadAsStringAsync();
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
