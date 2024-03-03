using OTelDemo.OtlpDebugger.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<LogsDebuggerService>();
app.MapGrpcService<MetricsDebuggerService>();
app.MapGrpcService<TracingDebuggerService>();

app.MapGet("/", () => "This is OTLP debugger.");

app.Run();
