using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using System.Diagnostics.Metrics;
using System.Text;

namespace OTelDemo.OtlpDebugger.Services
{
    public class MetricsDebuggerService : MetricsService.MetricsServiceBase
    {
        private ILogger<MetricsDebuggerService> logger;

        public MetricsDebuggerService(ILogger<MetricsDebuggerService> logger)
        {
            this.logger = logger;
        }

        public override Task<ExportMetricsServiceResponse> Export(ExportMetricsServiceRequest request, ServerCallContext context)
        {
            int count = request.ResourceMetrics.Sum(rl => rl.ScopeMetrics.Sum(s => s.Metrics.Count));
            logger.LogInformation("Metrics received. Count: {Count}", count);
            return Task.FromResult(new ExportMetricsServiceResponse());
        }
    }
}
