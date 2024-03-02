namespace OTelDemo.Web.Services
{
    public class WeatherApiClient
    {
        private readonly HttpClient httpClient;

        public WeatherApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetForecastAsync()
        {
            var forecasts = await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast")
                ?? throw new NullReferenceException("No data returned");

            var forecast = forecasts.First();

            return $"{forecast.Summary}, {forecast.TemperatureC}°C";
        }

        record WeatherForecast(DateOnly Date, int TemperatureC, string Summary);
    }
}
