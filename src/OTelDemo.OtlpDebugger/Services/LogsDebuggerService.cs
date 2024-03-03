using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Proto.Collector.Logs.V1;
using System.Text;

namespace OTelDemo.OtlpDebugger.Services
{
    public class LogsDebuggerService : LogsService.LogsServiceBase
    {
        private ILogger<LogsDebuggerService> logger;

        public LogsDebuggerService(ILogger<LogsDebuggerService> logger)
        {
            this.logger = logger;
        }

        public override Task<ExportLogsServiceResponse> Export(ExportLogsServiceRequest request, ServerCallContext context)
        {
            StringBuilder sb = new();
            foreach (var log in request.ResourceLogs)
            {
                //sb.AppendLine($"Got logs!");

                // collect resource attributes (e.g. service name, environment, etc.)
                foreach (var atr in log.Resource.Attributes)
                {
                    //sb.AppendLine($"- {atr.Key}: {atr.Value.StringValue}");
                }

                // collect log entries (e.g. log level, message, etc.)
                foreach (var entry in log.ScopeLogs)
                {
                    //sb.AppendLine($"- Log(s):");

                    foreach (var logEntry in entry.LogRecords)
                    {
                        DateTime time = DateTimeOffset
                            .FromUnixTimeMilliseconds((long)(logEntry.TimeUnixNano / 1_000_000))
                            .LocalDateTime;

                        sb.AppendLine($"Log {time}: [{logEntry.SeverityText}] {logEntry.Body.StringValue}");
                    }
                }
            }

            logger.LogInformation(sb.ToString());

            return Task.FromResult(new ExportLogsServiceResponse());
        }
    }
}
