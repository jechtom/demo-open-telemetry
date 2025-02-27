using OTelDemo.OtlpDebugger;
using OTelDemo.OtlpDebugger.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Logging.AddSimpleConsole(c =>
{
    c.SingleLine = true;
    c.IncludeScopes = false;
    c.TimestampFormat = "HH:mm:ss ";
});

var app = builder.Build();

app.MapGrpcService<LogsDebuggerService>();
app.MapGrpcService<MetricsDebuggerService>();
app.MapGrpcService<TracingDebuggerService>();

app.MapGet("/", () => "This is OTLP debugger.");

app.Run();
