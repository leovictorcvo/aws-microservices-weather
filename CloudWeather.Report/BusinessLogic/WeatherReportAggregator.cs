using System.Text.Json;
using CloudWeather.Report.Config;
using CloudWeather.Report.DataAccess;
using CloudWeather.Report.Models;
using Microsoft.Extensions.Options;

namespace CloudWeather.Report.BusinessLogic
{
    public class WeatherReportAggregator : IWeatherReportAggregator
    {
        private readonly IHttpClientFactory _http;
        private readonly ILogger<WeatherReportAggregator> _logger;
        private readonly WeatherDataConfig _weatherDataConfig;
        private readonly WeatherReportDbContext _db;

        public WeatherReportAggregator(
            IHttpClientFactory http, 
            ILogger<WeatherReportAggregator> logger,
            IOptions<WeatherDataConfig> weatherDataConfig,
            WeatherReportDbContext db
        )
        {
            _http = http;
            _logger = logger;
            _weatherDataConfig = weatherDataConfig.Value;
            _db = db;
        }

        public async Task<WeatherReport> BuildReport(string zip, int days)
        {
            var httpClient = _http.CreateClient();
            
            var precipitationData = await FetchPrecipitationData(httpClient, zip, days);
            var totalSnow = GetTotalByWeatherType(precipitationData, "snow");
            var totalRain = GetTotalByWeatherType(precipitationData, "rain");
            _logger.LogInformation($"zip:{zip} over last {days} days => total snow: {totalSnow}, rain: {totalRain}");

            var temperatureData = await FetchTemperatureData(httpClient, zip, days);
            var averageHighTemp = temperatureData.Average(t => t.TempHighF);
            var averageLowTemp = temperatureData.Average(t => t.TempLowF);
            _logger.LogInformation($"zip:{zip} over last {days} days => lo temp: {averageLowTemp}, hi temp: {averageHighTemp}");

            var weatherReport = new WeatherReport
            {
                AverageHighF = Math.Round(averageHighTemp, 1),
                AverageLowF = Math.Round(averageLowTemp, 1),
                RainfallTotalInches = totalRain,
                SnowTotalInches = totalSnow,
                ZipCode = zip,
                CreatedOn = DateTime.UtcNow
            };

            await _db.AddAsync(weatherReport);
            await _db.SaveChangesAsync();

            return weatherReport;
        }

        private decimal GetTotalByWeatherType(List<PrecipitationModel> precipitationData, string weatherType)
        {
            var total = precipitationData
                .Where(p => p.WeatherType.ToLower().Equals(weatherType.ToLower()))
                .Sum(p => p.AmountInches);
            return Math.Round(total, 1);
        }

        private async Task<List<TemperatureModel>> FetchTemperatureData(HttpClient httpClient, string zip, int days)
        {
            var endpoint = BuildTemperatureServiceEndpoint(zip, days);
            var temperatureRecords = await httpClient.GetAsync(endpoint);
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var temperatureData = await temperatureRecords.Content.ReadFromJsonAsync<List<TemperatureModel>>(jsonSerializerOptions);
            return temperatureData ?? new List<TemperatureModel>();
        }

        private string BuildTemperatureServiceEndpoint(string zip, int days)
        {
            var temperatureServiceProtocol = _weatherDataConfig.TemperatureDataProtocol;
            var temperatureServiceHost = _weatherDataConfig.TemperatureDataHost;
            var temperatureServicePort = _weatherDataConfig.TemperatureDataPort;
            return $"{temperatureServiceProtocol}://{temperatureServiceHost}:{temperatureServicePort}/observation/{zip}?days={days}";
        }

        private async Task<List<PrecipitationModel>> FetchPrecipitationData(HttpClient httpClient, string zip, int days)
        {
            var endpoint = BuildPrecipitationServiceEndpoint(zip, days);
            var precipitationRecords = await httpClient.GetAsync(endpoint);
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var precipitationData = await precipitationRecords.Content.ReadFromJsonAsync<List<PrecipitationModel>>(jsonSerializerOptions);
            return precipitationData ?? new List<PrecipitationModel>();       
        }
        private string BuildPrecipitationServiceEndpoint(string zip, int days)
        {
            var precipitationServiceProtocol = _weatherDataConfig.PrecipitationDataProtocol;
            var precipitationServiceHost = _weatherDataConfig.PrecipitationDataHost;
            var precipitationServicePort = _weatherDataConfig.PrecipitationDataPort;
            return $"{precipitationServiceProtocol}://{precipitationServiceHost}:{precipitationServicePort}/observation/{zip}?days={days}";
        }
    }
}