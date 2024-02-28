using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercise2.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> _logger;
    private readonly AppDbContext appDbContext;

    public WeatherController(ILogger<WeatherController> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        this.appDbContext = appDbContext;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherSerie>> Get()
    {
        _logger.LogInformation("Get requested");
        var weather = await appDbContext.Weather.Where(x=>x.Timestamp > DateTime.UtcNow.AddHours(-2)).ToListAsync();
        return weather.GroupBy(x => $"{x.Country} - {x.City}")
            .Select(s => new WeatherSerie()
            {
                Name = s.Key,
                Data = s.Select(i => new Data() { Clouds = i.Clouds, Temperature = i.Temperature, Time = i.Timestamp.ToShortTimeString(), WindSpeed = i.WindSpeed }).ToArray()
            });
    }
}

public record WeatherSerie
{
    public string Name { get; init; }
    public Data[] Data { get; init; }
}

public record Data
{
    public double Temperature { get; init; }
    public int Clouds { get; init; }
    public double WindSpeed { get; init; }
    public string Time { get; init; }
}

public record Weather
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public string Country { get; init; }
    public string City { get; init; }
    public double Temperature { get; init; }
    public int Clouds { get; init; }
    public double WindSpeed { get; init; }
    public DateTime Timestamp { get; init; }

    public Weather(string country, string city, double temperature, int clouds, double windSpeed, DateTime timestamp) : this(default, country, city, temperature, clouds, windSpeed, timestamp)
    {
    }

    public Weather(int id, string country, string city, double temperature, int clouds, double windSpeed, DateTime timestamp)
    {
        Id = id;
        Country = country;
        City = city;
        Temperature = temperature;
        Clouds = clouds;
        WindSpeed = windSpeed;
        Timestamp = timestamp;
    }

    private Weather() { }
}
