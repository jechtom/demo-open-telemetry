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
            StringBuilder sb = new();
            foreach (var span in request.ResourceSpans)
            {
                //sb.AppendLine($"Got spans!");

                // collect resource attributes (e.g. service name, environment, etc.)
                foreach (var atr in span.Resource.Attributes)
                {
                    //sb.AppendLine($"- {atr.Key}: {atr.Value.StringValue}");
                }

                // collect span data (e.g. span name, duration, etc.)
                foreach (var data in span.ScopeSpans)
                {
                    foreach (var spanData in data.Spans)
                    {
                        //sb.AppendLine($"- Span(s):");
                        sb.AppendLine($"Trace: {spanData.Name} {spanData.TraceId.ToHexString()}/{spanData.SpanId.ToHexString()}");
                    }
                }
            }

            logger.LogInformation(sb.ToString());

            return Task.FromResult(new ExportTraceServiceResponse());
        }
    }
}
