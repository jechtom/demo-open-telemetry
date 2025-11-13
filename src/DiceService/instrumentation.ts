import { diag, DiagConsoleLogger, DiagLogLevel } from '@opentelemetry/api';
import { NodeSDK } from '@opentelemetry/sdk-node';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';
import { PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-grpc';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-grpc';
import { ATTR_SERVICE_NAME, ATTR_SERVICE_VERSION } from '@opentelemetry/semantic-conventions';
import { resourceFromAttributes } from '@opentelemetry/resources';
import process from 'process';

diag.setLogger(new DiagConsoleLogger(), DiagLogLevel.INFO);

const otelEndpoint = process.env.OTEL_ENDPOINT || "http://localhost:4317";
console.log(`OTel Endpoint: ${otelEndpoint}`);
const sdk = new NodeSDK({
  resource: resourceFromAttributes({
    [ ATTR_SERVICE_NAME ]: "DiceService",
    [ ATTR_SERVICE_VERSION ]: "1.0",
  }),
  traceExporter: new OTLPTraceExporter({
    url: otelEndpoint
  }),
  metricReaders: [ new PeriodicExportingMetricReader({
    exporter: new OTLPMetricExporter({
      url: otelEndpoint
    }),
    exportIntervalMillis: 5000
  })],
  instrumentations: [getNodeAutoInstrumentations()],
});

sdk.start();