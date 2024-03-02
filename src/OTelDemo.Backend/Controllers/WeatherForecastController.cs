using Microsoft.AspNetCore.Mvc;

namespace OTelDemo.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            using (var activity = AppActivitySource.Current.StartActivity("GetWeatherForecast"))
            {
                _logger.LogInformation("Getting weather forecast");

                await DoSomethingUseless();
                
                await Task.Delay(20); // thinking...

                _logger.LogInformation("Got weather forecast");

                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            }
        }

        private async Task DoSomethingUseless()
        {
            using (var activity = AppActivitySource.Current.StartActivity("UselessActivity"))
            {
                await Task.Delay(Random.Shared.Next(40, 60));
            }
        }
    }
}
