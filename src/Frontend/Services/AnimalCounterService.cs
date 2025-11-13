using System.Diagnostics.Metrics;

namespace OTelDemo.Web.Services
{
    public class AnimalCounterService
    {
        private static readonly Counter<int> animalCounter =
            ObservabilitySource.Meter.CreateCounter<int>("animal.count");

        public void IncrementAnimalEmoji()
        {
            animalCounter.Add(1, new KeyValuePair<string, object?>("animal", "🐼"));
            animalCounter.Add(1, new KeyValuePair<string, object?>("animal", "🐶"));
            animalCounter.Add(1, new KeyValuePair<string, object?>("animal", "🐍"));
            animalCounter.Add(1, new KeyValuePair<string, object?>("animal", "🐈"));
        }
    }
}
