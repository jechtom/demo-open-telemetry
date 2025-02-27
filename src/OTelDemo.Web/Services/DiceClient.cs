namespace OTelDemo.Web.Services
{
    public class DiceClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<DiceClient> logger;

        public DiceClient(HttpClient httpClient, ILogger<DiceClient> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<int> RollTheDice()
        {
            var result = await httpClient.GetFromJsonAsync<int>("rolldice");
            logger.LogInformation("Result from nodejs-dice service: {Result}", result);
            return result;
        }
    }
}
