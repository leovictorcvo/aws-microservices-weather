using CloudWeather.Precipitation.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PrecipitationDbContext>(opts =>
{
    opts.EnableSensitiveDataLogging();
    opts.EnableDetailedErrors();
    opts.UseNpgsql(builder.Configuration.GetConnectionString("AppDB"));
}, ServiceLifetime.Scoped);

var app = builder.Build();

const string baseRoute = "/observation";

app.MapGet($"{baseRoute}/{{zip}}", async (string zip, [FromQuery] int? days, PrecipitationDbContext db) =>
{
    if (!days.HasValue || days.Value < 1 || days.Value > 30) return Results.BadRequest("Please provide a 'days' query parameter between 1 and 30");

    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var results = await db.Precipitation.Where(x => x.ZipCode.Equals(zip) && x.CreatedOn > startDate).ToListAsync();

    return Results.Ok(results);
});

app.MapPost(baseRoute, async (PrecipitationPostModel model, PrecipitationDbContext db) => 
{
    var precipitation = new Precipitation(model);
    await db.AddAsync(precipitation);
    await db.SaveChangesAsync();
    return Results.Created($"{baseRoute}/{precipitation.ZipCode}", precipitation);
});

app.Run();
