using System.Diagnostics;

namespace OTelDemo.Web.Services
{
    public class MessageGenerator
    {
        public async Task<string> GenerateMessageAsync()
        {
            using (var activity = AppActivitySource.Current.StartActivity("GenerateMessage"))
            {
                activity?.SetTag("foo", "bar1");
                await Task.Delay(Random.Shared.Next(20, 50)); // thinking...
                activity?.AddEvent(new ActivityEvent("Part way there"));
                await Task.Delay(Random.Shared.Next(20, 50)); // thinking...
                activity?.AddEvent(new ActivityEvent("Done"));
                return "Hello, World!";
            }
        }
    }
}
