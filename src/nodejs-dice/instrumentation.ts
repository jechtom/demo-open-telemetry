import * as opentelemetry from '@opentelemetry/sdk-node';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-grpc';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-grpc';
import { PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import { diag, DiagConsoleLogger, DiagLogLevel } from '@opentelemetry/api';
import { Resource } from '@opentelemetry/resources';

diag.setLogger(new DiagConsoleLogger(), DiagLogLevel.INFO);

export const OTEL_SERVICE_RESOURCE = new Resource({
    'service.name': 'nodejs-dice'
  });

const otelEndpoint = "http://localhost:4317"

const sdk = new opentelemetry.NodeSDK({
  resource: OTEL_SERVICE_RESOURCE,
  traceExporter: new OTLPTraceExporter({
    url: otelEndpoint
  }),
  metricReader: new PeriodicExportingMetricReader({
    exporter: new OTLPMetricExporter({
      url: otelEndpoint
    }),
  }),
  instrumentations: [getNodeAutoInstrumentations()],
});
sdk.start();