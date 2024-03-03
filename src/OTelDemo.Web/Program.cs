using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.Metrics;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OTelDemo.Web;
using OTelDemo.Web.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// define the resource (e.g. service name, environment, etc.)
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(builder.Environment.ApplicationName);

// define the OTLP endpoint URI
var otlpEndpoint = new Uri("http://localhost:4017"); // connect to the OptlDebugger
//var otlpEndpoint = new Uri("http://localhost:4317"); // connect to the OtelCollector

// configure OpenTelemetry for logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.SetResourceBuilder(resourceBuilder)
        .AddOtlpExporter(c => {
            c.Endpoint = otlpEndpoint;
        });
});

// debug print: activity sources
ActivitySource.AddActivityListener(new ActivityListener()
{
    ShouldListenTo = (activitySource) =>
    {
        Console.WriteLine("Activity source: " + activitySource.Name);
        return false;
    }
});


builder.Services
    .AddOpenTelemetry()
    .WithMetrics(c =>
    {
        c.SetResourceBuilder(resourceBuilder);
        c.AddProcessInstrumentation();
        c.AddRuntimeInstrumentation();
        c.AddAspNetCoreInstrumentation();
        c.AddHttpClientInstrumentation();
        c.AddMeter("OTelDemo.Web");

        //c.AddEventCountersInstrumentation(c => // adds older EventCounter support
        //{
        //    c.AddEventSources("Microsoft.AspNetCore.Hosting");
        //    c.RefreshIntervalSecs = 5;
        //});

        //c.AddAzureMonitorMetricExporter(o => o.ConnectionString = ".....");

        c.AddPrometheusExporter();

        c.AddOtlpExporter(o =>
        {
            o.Endpoint = otlpEndpoint;
        });
    })
    .WithTracing(c =>
    {
        c.AddEntityFrameworkCoreInstrumentation();
        c.AddSource("OTelDemo.Web");
        c.AddSource("Npgsql");
        c.SetResourceBuilder(resourceBuilder);
        c.AddAspNetCoreInstrumentation();
        c.AddHttpClientInstrumentation();
        c.AddOtlpExporter(o =>
        {
            o.Endpoint = otlpEndpoint;
        });
    });


builder.Services.AddScoped<MessageGenerator>();

builder.Services.AddHttpClient<WeatherApiClient>(c =>
{
    c.BaseAddress = new Uri("http://localhost:4006");
});

builder.Services.AddDbContext<DemoDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Db"));
});

builder.Services.AddRazorPages();

var app = builder.Build();

app.Logger.LogInformation("Application is starting.");

// create the database if it doesn't exist
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<DemoDbContext>();
    await context.Database.EnsureCreatedAsync();
    await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
