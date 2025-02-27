using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Proto.Collector.Trace.V1;
using System.Text;

namespace OTelDemo.OtlpDebugger.Services
{
    public class TracingDebuggerService : TraceService.TraceServiceBase
    {
        private ILogger<TracingDebuggerService> logger;

        public TracingDebuggerService(ILogger<TracingDebuggerService> logger)
        {
            this.logger = logger;
        }

        public override Task<ExportTraceServiceResponse> Export(ExportTraceServiceRequest request, ServerCallContext context)
        {
            int count = request.ResourceSpans.Sum(rl => rl.ScopeSpans.Sum(s => s.Spans.Count));
            logger.LogInformation("Tracing received. Count: {Count}", count);
            return Task.FromResult(new ExportTraceServiceResponse());
        }
    }
}
