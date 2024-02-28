using System.Text.Json;
using Exercise2.API;
using Exercise2.Controllers;

public class WeatherBackgroundService : BackgroundService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WeatherBackgroundService> _logger;

    public WeatherBackgroundService(IHttpClientFactory clientFactory, IServiceProvider serviceProvider, ILogger<WeatherBackgroundService> logger)
    {
        this._httpFactory = clientFactory;
        this._serviceProvider = serviceProvider;
        this._logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var locations = new List<(string Country, string City)>
        {
            ("pl", "Warsaw"),
            ("gr", "Chania"),
            ("it", "Palermo"),
            ("hu", "Budapest"),
            ("hr", "Split"),
            ("cy", "Larnaca"),
            ("uk", "London"),
            ("nl", "Tilburg"),
            ("es", "Sevilla"),
            ("tr", "Antalya"),
        };

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                _logger.LogInformation($"{nameof(WeatherBackgroundService)} execution started");
                foreach (var (Country, City) in locations)
                {
                    var client = _httpFactory.CreateClient();
                    var response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={City},{Country}&units=metric&APPID=75e3e4942cad2e9d2c2742978701caac", stoppingToken);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync(stoppingToken);
                        var weatherInfo = JsonSerializer.Deserialize<OpenWeatherResponse>(content) ??
                            throw new Exception("Couldn't fetch weather from OpenWeather");

                        var weatherData = new Exercise2.Controllers.Weather(Country,
                            City,
                            weatherInfo.main.temp,
                            weatherInfo.clouds.all,
                            weatherInfo.wind.speed,
                            DateTime.UtcNow);

                        dbContext.Weather.Add(weatherData);
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                    else
                    {
                        _logger.LogInformation($"{nameof(WeatherBackgroundService)} execution FAILED, StatusCode:{response.StatusCode}, Message: {response.RequestMessage}");
                    }
                }
            }
            var interval = TimeSpan.FromMinutes(1);
            _logger.LogInformation($"{nameof(WeatherBackgroundService)} execution finished. Waiting interval: {interval}");
            await Task.Delay(interval, stoppingToken);
        }
    }
}

