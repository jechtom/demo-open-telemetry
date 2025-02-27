using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.Metrics;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OTelDemo.Web;
using OTelDemo.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// configure OpenTelemetry
//builder.Services.AddOpenTelemetry()
//    .ConfigureResource(r => r.AddService("OTelDemo.Web", serviceVersion: "1.2.3.4"))
//    //.UseOtlpExporter(OtlpExportProtocol.Grpc, new Uri("http://localhost:4017")) // OTLP debugger
//    //.UseOtlpExporter(OtlpExportProtocol.Grpc, new Uri("http://localhost:4317")) // default OTLP/GRPC port
//    //.UseOtlpExporter(OtlpExportProtocol.HttpProtobuf, new Uri("http://localhost:4002/ingest/otlp")) // SEQ
//    //.UseOtlpExporter(OtlpExportProtocol.HttpProtobuf, new Uri("http://localhost:4003/api/v1/otlp")) // Prometheus
//    .WithLogging()
//    .WithTracing(c =>
//    {
//        c.AddEntityFrameworkCoreInstrumentation();
//        c.AddSource("OTelDemo.Web");
//        c.AddSource("Npgsql");
//        c.AddAspNetCoreInstrumentation();
//        c.AddHttpClientInstrumentation();
//    })
//    .WithMetrics(c =>
//    {
//        c.AddProcessInstrumentation();
//        c.AddRuntimeInstrumentation();
//        c.AddAspNetCoreInstrumentation();
//        c.AddHttpClientInstrumentation();
//        c.AddMeter("OTelDemo.Web");
//        c.AddPrometheusExporter();
//    })
//    ;


builder.Services.AddScoped<MessageGenerator>();

builder.Services.AddHttpClient<WeatherApiClient>(c =>
{
    c.BaseAddress = new Uri("http://localhost:4006");
});

builder.Services.AddHttpClient<DiceClient>(c =>
{
    c.BaseAddress = new Uri("http://localhost:8080");
});

builder.Services.AddDbContext<DemoDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Db"));
});

builder.Services.AddRazorPages();

builder.Services.AddHttpLogging(c => { });

using var app = builder.Build();

app.Logger.LogInformation("Application is starting.");

// create the database if it doesn't exist
using (var serviceScope = app.Services.CreateScope())
{
    try
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<DemoDbContext>();
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();
    }
    catch(Exception e)
    {
        app.Logger.LogCritical(e, "Failed to init DB.");
        throw;
    }
}

// Configure the HTTP request pipeline.
app.UseHttpLogging();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
