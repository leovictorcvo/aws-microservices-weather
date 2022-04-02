using CloudWeather.Report.BusinessLogic;
using CloudWeather.Report.Config;
using CloudWeather.Report.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherReportAggregator, WeatherReportAggregator>();
builder.Services.AddOptions();
builder.Services.Configure<WeatherDataConfig>(builder.Configuration.GetSection("WeatherDataConfig"));
builder.Services.AddDbContext<WeatherReportDbContext>(opts =>
{
    opts.EnableSensitiveDataLogging();
    opts.EnableDetailedErrors();
    opts.UseNpgsql(builder.Configuration.GetConnectionString("AppDB"));
}, ServiceLifetime.Scoped);

var app = builder.Build();

app.MapGet(
    "/weather-report/{zip}", 
    async (string zip, [FromQuery] int? days, IWeatherReportAggregator aggregator) => 
    { 
        if (!days.HasValue || days.Value < 1 || days.Value > 30) return Results.BadRequest("Please provide a 'days' query parameter between 1 and 30");

        var report = await aggregator.BuildReport(zip, days.HasValue ? days.Value : 1);
        return Results.Ok(report);
    }
);

app.Run();
