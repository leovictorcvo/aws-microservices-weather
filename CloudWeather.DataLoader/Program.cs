using CloudWeather.DataLoader.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var servicesConfig = config.GetSection("Services");

var temperatureServiceConfig = servicesConfig.GetSection("Temperature");
var temperatureServiceHost = temperatureServiceConfig["Host"];
var temperatureServicePort = temperatureServiceConfig["Port"];

var precipitationServiceConfig = servicesConfig.GetSection("Preciptation");
var precipitationServiceHost = precipitationServiceConfig["Host"];
var precipitationServicePort = precipitationServiceConfig["Port"];

var temperatureHttpClient = new HttpClient()
{
    BaseAddress = new Uri($"http://{temperatureServiceHost}:{temperatureServicePort}")
};
var precipitationHttpClient = new HttpClient()
{
    BaseAddress = new Uri($"http://{precipitationServiceHost}:{precipitationServicePort}")
};

var zipCodes = new List<string>() { "73026", "68104", "04401", "32808", "19717" };

foreach(string zip in zipCodes)
{
    Console.WriteLine($"Processing Zip Code: {zip}");
    var from = DateTime.Now.AddYears(-2);
    var thru = DateTime.Now;

    for(var day = from.Date; day.Date < thru.Date; day = day.AddDays(1))
    {
        var temps = await PostTemperatureAsync(zip, day, temperatureHttpClient);
        await PostPrecipitationAsync(temps[0], zip, day, precipitationHttpClient);
    }
}

async Task<List<int>> PostTemperatureAsync(string zip, DateTime day, HttpClient temperatureHttpClient)
{
    var rand = new Random();
    var t1 = rand.Next(0,100);
    var t2 = rand.Next(0,100);
    var hiLoTemps = new List<int> { t1, t2 };
    hiLoTemps.Sort();

    var temperatureObservation = new TemperatureModel()
    {
        TempLowF = hiLoTemps[0],
        TempHighF = hiLoTemps[1],
        ZipCode = zip,
        CreatedOn = day,
    };

    var temperatureResponse = await temperatureHttpClient.PostAsJsonAsync("observation", temperatureObservation);

    Console.WriteLine($"Posted Temperature => Date: {day:d} - Zip: {zip} - Lo (F): {hiLoTemps[0]} - Hi (F):{hiLoTemps[1]}");
    if (temperatureResponse.IsSuccessStatusCode)
    {
        Console.WriteLine("Post Ok");
    }
    else
    {
        Console.WriteLine($"Posted Temperature Failed - {temperatureResponse }");
    }
    return hiLoTemps;
}

async Task PostPrecipitationAsync(int tempLow, string zip, DateTime day, HttpClient precipitationHttpClient)
{
    var rand = new Random();
    var isPrecip = rand.Next(2) < 1;

    var preciptationInches = isPrecip ? rand.Next(1, 16) : 0;
    var weatherType = !isPrecip ? "none" : tempLow < 32 ? "snow" : "rain";
    
    PreciptationModel preciptation = new PreciptationModel
    {
        AmountInches = preciptationInches,
        WeatherType = weatherType,
        ZipCode = zip,
        CreatedOn = day,
    };

    var preciptationResponse = await precipitationHttpClient.PostAsJsonAsync("observation", preciptation);

    Console.WriteLine($"Posted Preciptation => Date: {day:d} - Zip: {zip} - Type: {weatherType} - Amount(in.):{preciptationInches}");
    if (preciptationResponse.IsSuccessStatusCode)
    {
        Console.WriteLine("Post Ok");
    }
    else
    {
        Console.WriteLine($"Posted Preciptation Failed - {preciptationResponse}");
    }
}