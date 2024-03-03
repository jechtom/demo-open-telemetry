# WUG Dev Day 2024-03-04 OpenTelemetry Demo

## Prereq

* Docker (with docker-compose) is installed and started.

# Demo A - Under the Hood

## Step 1 - Prepare Background Services

```
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
# explore collector configs variants
# explore: core: https://github.com/open-telemetry/opentelemetry-collector
# explore: contrib: https://github.com/open-telemetry/opentelemetry-collector-contrib
docker-compose up otel-collector
```

## Step 2 - Update and Run Demo App

```
# explore config, set OTLP endpoint to collector endpoint
dotnet run --project .\src\OTelDemo.Web\OTelDemo.Web.csproj
# then explore: https://localhost:7044
```