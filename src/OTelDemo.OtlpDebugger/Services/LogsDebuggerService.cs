using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Proto.Collector.Logs.V1;
using System.Diagnostics.Metrics;
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
            int count = request.ResourceLogs.Sum(rl => rl.ScopeLogs.Sum(s => s.LogRecords.Count));
            logger.LogInformation("Log records received. Count: {Count}", count);
            return Task.FromResult(new ExportLogsServiceResponse());
        }
    }
}
