namespace OTelDemo.Web.Services
{
    public class OrderApiClient
    {
        private readonly HttpClient httpClient;

        public OrderApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task ProcessOrderAsync()
        {
            var order = new Order(
                Lines: [
                    new OrderLine(2, 1),
                    new OrderLine(1, 2),
                ],
                Email: "customer@example.com",
                FirstName: "John",
                LastName: "Doe"
            );

            (await httpClient.PostAsJsonAsync("Order", order))
                .EnsureSuccessStatusCode();
        }

        record WeatherForecast(DateOnly Date, int TemperatureC, string Summary);
    }

    public record OrderLine(int Quantity, int ProductId);
    public record Order(OrderLine[] Lines, string Email, string FirstName, string LastName);
}
