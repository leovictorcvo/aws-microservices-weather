using CloudWeather.Temperature.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TemperatureDbContext>(opts =>
{
    opts.EnableSensitiveDataLogging();
    opts.EnableDetailedErrors();
    opts.UseNpgsql(builder.Configuration.GetConnectionString("AppDB"));
}, ServiceLifetime.Scoped);

var app = builder.Build();

const string baseRoute = "/temperature";

app.MapGet($"{baseRoute}/{{zip}}", async (string zip, [FromQuery] int? days, TemperatureDbContext db) =>
{
    if (!days.HasValue || days.Value < 1 || days.Value > 30) return Results.BadRequest("Please provide a 'days' query parameter between 1 and 30");

    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var results = await db.Temperature.Where(x => x.ZipCode.Equals(zip) && x.CreatedOn > startDate).ToListAsync();

    return Results.Ok(results);
});

app.MapPost(baseRoute, async (TemperaturePostModel model, TemperatureDbContext db) => 
{
    var temperature = new Temperature(model);
    await db.AddAsync(temperature);
    await db.SaveChangesAsync();
    return Results.CreatedAtRoute($"{baseRoute}/{temperature.ZipCode}", temperature);
});

app.Run();
