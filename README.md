# 2025 OpenTelemetry Demo

By [@jechtom](https://github.com/jechtom)

## Endpoints

| Service Name         | URL Address                    |
|----------------------|--------------------------------|
| Frontend service     | https://localhost:7044         |
| Backend service      | http://localhost:4006          |
| My OTLP debugger     | http://localhost:4017          |
| Prometheus (metrics) | http://localhost:4003          |
| Jaeger (tracing)     | http://localhost:4004          |
| Zipkin (tracing)     | http://localhost:4005          |
| SEQ (logging)        | http://localhost:4002          |
| Aspire dashboard     | http://localhost:18888         |

## Prereq

* Docker (with docker-compose) is installed and started.
* .NET 9 SDK to build and run local apps

## Prepare

```
# pull and build images
docker compose pull
docker compose build

cd src/nodejs-dice
npm install
npx ts-node --require ./instrumentation.ts app.ts
```

## DEMO - Examples of Backends Supporting OpenTelemetry Protocol (OTLP)

.NET Aspire Dashboard + Jaeger, Zipkin, Prometheus, SEQ, …

```
docker compose up db prometheus jaeger zipkin seq aspire otlp-debugger backend -d
# show OTLP debugger
# show different tools - Aspire dashboard, SEQ, Prometheus, Zipkin, Jaeger... - no data
```

## DEMO - OpenTelemetry .NET SDK - Logs, Traces
```
# show & run OTelDemo.Web
# enable telemetry
# show .NET Aspire (logs, metrics, tracing)
# switch connection to SEQ - show SEQ (logs, tracing)
```

## DEMO - Metrics
```
# explore metrics in Aspire
# redirect to Prometheus, show metrics in Prometheus
```

## DEMO - OpenTelemetry Collector
```
# show config and run otel-collector
# show registry (collector): https://opentelemetry.io/ecosystem/registry/?language=collector 
# stop Aspire (same port in use)
docker compose up otel-collector -d
# run web
# -- show trace and logs over all systems (SEQ, Jaeger, Zipkin)
```

## DEMO - Zero Code Instrumentation
```
# show empty missing tracing and logging from OTelDemo.Backend
# in docker-compose.yaml enable auto instrumentation for _backend_
# delete and recreate backend
# repeat frontend request and inspect new trace
```