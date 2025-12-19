# Observability

## ADDED Requirements

### Requirement: OpenTelemetry Tracing
The application MUST be instrumented with OpenTelemetry to emit traces for key operations.

#### Scenario: Outgoing HTTP Requests
Given the server is running with OTel configured
When a tool executes an action that calls the Mealie API
Then a trace span MUST be generated for the outgoing HTTP request.

#### Scenario: OTLP Export
Given the environment variable `OTEL_EXPORTER_OTLP_ENDPOINT` is set
When the application runs
Then traces and metrics MUST be exported to the specified endpoint via OTLP.

### Requirement: Structured Logging
The application MUST integrate OpenTelemetry with the logging provider.

#### Scenario: Log Correlation
Given a request is being traced
When a log message is written
Then the log entry MUST contain the `TraceId` and `SpanId` of the current active trace context.
