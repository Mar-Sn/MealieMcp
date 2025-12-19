# Tasks: OpenTelemetry Support

1.  **Dependencies**
    -   [ ] Add `OpenTelemetry.Extensions.Hosting` package. <!-- id: 0 -->
    -   [ ] Add `OpenTelemetry.Instrumentation.Http` package. <!-- id: 1 -->
    -   [ ] Add `OpenTelemetry.Instrumentation.Runtime` package. <!-- id: 8 -->
    -   [ ] Add `OpenTelemetry.Exporter.OpenTelemetryProtocol` package. <!-- id: 2 -->

2.  **Implementation**
    -   [ ] Update `Program.cs` to configure OpenTelemetry Tracing (add source `MealieMcp`, instrument HttpClient, add OTLP exporter). <!-- id: 3 -->
    -   [ ] Update `Program.cs` to configure OpenTelemetry Metrics (instrument Runtime, HttpClient, add OTLP exporter). <!-- id: 4 -->
    -   [ ] Update `Program.cs` to configure OpenTelemetry Logging. <!-- id: 5 -->
    -   [ ] Update `Dockerfile` to expose `OTEL_` environment variables (e.g., `OTEL_EXPORTER_OTLP_ENDPOINT`, `OTEL_SERVICE_NAME`). <!-- id: 9 -->

3.  **Validation**
    -   [ ] Verify the application builds with new dependencies. <!-- id: 6 -->
    -   [ ] (Manual) Verify traces are emitted when `OTEL_EXPORTER_OTLP_ENDPOINT` is set (e.g. to a local Aspire dashboard or Jaeger). <!-- id: 7 -->
