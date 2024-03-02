using System.Diagnostics;

namespace OTelDemo.Web
{
    public static class AppActivitySource
    {
        public static ActivitySource Current { get; } = new ActivitySource("OTelDemo.Web");
    }
}
