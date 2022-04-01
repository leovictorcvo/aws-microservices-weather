using CloudWeather.Report.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WeatherReportDbContext>(opts =>
{
    opts.EnableSensitiveDataLogging();
    opts.EnableDetailedErrors();
    opts.UseNpgsql(builder.Configuration.GetConnectionString("AppDB"));
}, ServiceLifetime.Scoped);

var app = builder.Build();

app.Run();
