using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OTelDemo.Web.Services
{
    public class MessageGenerator
    {
        static Counter<int> generatedMessagesCounter = ObservabilitySource.Meter.CreateCounter<int>("message.generated.count");
        private readonly ILogger<MessageGenerator> logger;

        public MessageGenerator(ILogger<MessageGenerator> logger)
        {
            this.logger = logger;
        }

        public async Task<string> GenerateMessageAsync()
        {
            generatedMessagesCounter.Add(1);

            using (var activity = ObservabilitySource.ActivitySource.StartActivity("GenerateMessage"))
            {
                activity?.SetTag("foo", "bar1");
                await Task.Delay(Random.Shared.Next(20, 50)); // thinking...
                activity?.AddEvent(new ActivityEvent("Part way there"));
                await Task.Delay(Random.Shared.Next(20, 50)); // thinking...
                activity?.AddEvent(new ActivityEvent("Done"));

                logger.LogInformation("Fresh hello message has been generated.");

                return "Hello, World!";
            }
        }
    }
}
