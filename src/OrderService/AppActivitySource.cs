using System.Diagnostics;

namespace OTelDemo.Backend
{
    public static class AppActivitySource
    {
        public static ActivitySource Current { get; } = new ActivitySource("OTelDemo.Backend");
    }
}
