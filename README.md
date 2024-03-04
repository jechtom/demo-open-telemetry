# WUG Dev Day 2024-03-04 OpenTelemetry Demo

## Prereq

* Docker (with docker-compose) is installed and started.
* .NET 8 SDK to build and run local apps

# Demo A - Under the Hood

## Step 1 - Prepare Background Services

```
docker compose pull
docker compose build
docker compose up db prometheus jaeger zipkin seq -d
```

## Step 2 - Start Demo OTLP Debugger

Note: Start in dedicated terminal window.

```
# explore project: src/OTelDemo.OtlpDebugger
docker-compose up otlp-debugger
```

## Step 3 - Run Serilog Demo

```
# enable OpenTelemetry sink in Program.cs
dotnet run --project .\src\OTelDemo.SerilogDemo\OTelDemo.SerilogDemo.csproj
```

# Demo B - .NET SDKs

## Step 1 - Start Backend Service

Note: Start in dedicated terminal window.

```
docker-compose up backend
# then explore: http://localhost:4006/swagger
```

## Step 2 - Start Demo App

```
# explore config, set OTLP endpoint to debugger endpoint
dotnet run --project .\src\OTelDemo.Web\OTelDemo.Web.csproj
# then explore: https://localhost:7044
# then explore backend service and otlp-debugger service logs
# then explore: https://opentelemetry.io/ecosystem/registry
```

# Demo C - OpenTelemetry Collector

## Step 1 - Explore Collector Config

Note: Start in dedicated terminal window.
```
# explore collector configs variants in src/otel-collector/
# explore: core: https://github.com/open-telemetry/opentelemetry-collector
# explore: contrib: https://github.com/open-telemetry/opentelemetry-collector-contrib
docker-compose up otel-collector
```

## Step 2 - Update and Run Demo App

```
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

## Step 1 - Update Docker Compose

Enable auto instrumentation for _backend_ service.

## Step 2 - Restart Backend

```
# stop running backend service Ctrl+C
docker-compose up backend
# then explore: https://localhost:7044
# then explore new service in: seq at: http://localhost:4002/
# then explore new service in: jaeger at: http://localhost:4004/
```