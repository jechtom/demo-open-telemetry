using OpenTelemetry.Metrics;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OTelDemo.Web
{
    public static class ObservabilitySource
    {
        public static ActivitySource ActivitySource { get; } = new ActivitySource("Frontend");
        public static Meter Meter { get; } = new Meter("Frontend");
    }
}
