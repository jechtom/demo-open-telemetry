using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Proto.Collector.Metrics.V1;
using System.Text;

namespace OTelDemo.OtplDebugger.Services
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
            StringBuilder sb = new();
            foreach (var metric in request.ResourceMetrics)
            {
                //sb.AppendLine($"Got metrics!");

                // collect resource attributes (e.g. service name, environment, etc.)
                foreach (var atr in metric.Resource.Attributes)
                {
                    //sb.AppendLine($"- {atr.Key}: {atr.Value.StringValue}");
                }

                // collect metric data (e.g. metric name, value, etc.)
                foreach (var data in metric.ScopeMetrics)
                {
                    foreach (var metricData in data.Metrics)
                    {
                        //sb.AppendLine($"- Metric(s):");
                        sb.AppendLine($"Metric: [{metricData.DataCase}] {metricData.Name}");
                    }
                }
            }

            logger.LogInformation(sb.ToString());

            return Task.FromResult(new ExportMetricsServiceResponse());
        }
    }
}
