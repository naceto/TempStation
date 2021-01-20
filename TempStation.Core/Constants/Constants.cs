namespace TempStation.Core.Constants
{
    public static class Constants
    {
        public static readonly string OpenWeatherMapHttpClientName = nameof(OpenWeatherMapHttpClientName);
        public static readonly string OpenWeatherMapConfigBaseUrl = "OpenWeatherMap:BaseUrl";
        public static readonly string OpenWeatherMapConfigApiKey = "OpenWeatherMap:ApiKey";
        public static readonly string OpenWeatherMapConfigCityId = "OpenWeatherMap:CityId";

        public static readonly string EntityCoreMigrationAssembly = "TempStation.Data";
        public static readonly string DefaultConnectionStringConfigName = "DefaultConnection";
        public static readonly string TempStationIdentityDbContextConnectionConfigName = "DefaultConnection";
    }
}
