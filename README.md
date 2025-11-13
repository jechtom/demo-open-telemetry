# 2025 OpenTelemetry Demo

By [@jechtom](https://github.com/jechtom)

## Endpoints

## Prereq

* Docker (with docker-compose) is installed and started.
* .NET 9 SDK to build and run local apps

## Prepare

```
# pull and build images
docker-compose pull
docker-compose build

# prep guest mode browser
# open links.html
```

## DEMO - Demo App Introduction

- Show app projects (Frontend, OrderService, DiceService, docker-compose.yaml)
- Start Application services (comments in docker-compose.yaml)
- Start app http://localhost:4000 - slow, but should work

## DEMO - Telemetry Backends + .NET Aspire

- Start Telemetry backend services (comments in docker-compose.yaml)
- open `links.html`, show backends

## DEMO - Collecting Logs & Traces

- Show apps OTEL integration - frontend + dice-service
  - Use of instrumentation librarires in both languages (PostgreSQL, HttpClient, etc.)
  - MessageGenerator - custom span
- Enable env: dice-service --> send to aspire 
- Enable env: frontend --> send to aspire
- `docker-compose up frontend dice-service -d`
- run app localhost:4000, show logs and traces 
  - filters, diagnostics (order-service double call), custom span (GenerateMessage), traceId

## DEMO - Metrics

- Show metrics in Aspire
- Change OTEL endpoint Frontend to Prometheus
- `docker-compose up frontend -d --force-recreate`
- Open Grafana http://localhost:4008/ - admin/admin/ok/skip
- Add data source: Prometheus http://prometheus:9090/
- Add graph `http_client_open_connections`, label by `server_address`

## DEMO - OpenTelemetry Collector

- show registry (collector): https://opentelemetry.io/ecosystem/registry/?language=collector 
- show config 
- `docker-compose up otel-collector -d` 
- update `docker-compose.yaml` to use otel-collector endpoint (commented)
- `docker-compose up frontend dice-service -d --force-recreate`
- show SEQ - logs and traces
- show Jaeger (search with traceId)
- show Aspire  (search with traceId)
- exaplain filters and extensions (collector)

## DEMO - Zero Code Instrumentation

- show empty missing tracing and logging from OrderService
- in docker-compose.yaml enable auto instrumentation for _OrderService_
- `docker-compose up order-service -d --force-recreate`
- repeat frontend request and inspect new trace
