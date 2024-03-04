# WUG Dev Day 2024-03-04 OpenTelemetry Demo

## Endpoints

| Service Name         | URL Address                    |
|----------------------|--------------------------------|
| Frontend service     | https://localhost:7044         |
| Backend service      | http://localhost:4006/swagger  |
| My OTLP debugger     | http://localhost:4017          |
| Prometheus (metrics) | http://localhost:4003          |
| Jaeger (tracing)     | http://localhost:4004          |
| Zipkin (tracing)     | http://localhost:4005          |
| SEQ (logging)        | http://localhost:4002          |

## Prereq

* Docker (with docker-compose) is installed and started.
* .NET 8 SDK to build and run local apps

## Prepare

```
# pull and build images
docker compose pull
docker compose build
```

## Demo A - Under the Hood

```
# Explore project: src/OTelDemo.OtlpDebugger 

# In dedicated window run:
docker-compose up otlp-debugger

# Explore and run project:
dotnet run --project .\src\OTelDemo.SerilogDemo\OTelDemo.SerilogDemo.csproj

# Enable OpenTelemetry sink in `.\src\OTelDemo.SerilogDemo\Program.cs`

# Re-run and view debugger console
dotnet run --project .\src\OTelDemo.SerilogDemo\OTelDemo.SerilogDemo.csproj
```

# Demo B - .NET SDKs

```
# start background services
docker compose up db prometheus jaeger zipkin seq -d

# Start in dedicated terminal window.
docker-compose up backend

# Explore: http://localhost:4006/swagger

# Explore OTelDemo.Web project
# - set OTLP endpoint to debugger endpoint
dotnet run --project .\src\OTelDemo.Web\OTelDemo.Web.csproj

# then explore: https://localhost:7044
# then explore backend service and otlp-debugger logs
# then explore: https://opentelemetry.io/ecosystem/registry
```

# Demo C - OpenTelemetry Collector

```
# explore collector configs variants in src/otel-collector/
# explore: core: https://github.com/open-telemetry/opentelemetry-collector
# explore: contrib: https://github.com/open-telemetry/opentelemetry-collector-contrib

# Start in dedicated terminal window.
docker-compose up otel-collector

# explore config, set OTLP endpoint to collector endpoint
dotnet run --project .\src\OTelDemo.Web\OTelDemo.Web.csproj

# then explore: https://localhost:7044
# then explore: seq at: http://localhost:4002/
# then explore: jaeger at: http://localhost:4004/
# then explore: zipkin at: http://localhost:4005/
# then explore: prometheus at: http://localhost:4003/ 
#  - try find custom metric: message_generated_count_total
# then explore Azure Monitor - metrics, application map, logs (traces)
```

# Demo D - Zero Code Instrumentation

```
# in docker-compose.yaml enable auto instrumentation for _backend_

# stop running backend service Ctrl+C
docker-compose up backend
# then explore: https://localhost:7044
# then explore new service in: seq at: http://localhost:4002/
# then explore new service in: jaeger at: http://localhost:4004/
```
