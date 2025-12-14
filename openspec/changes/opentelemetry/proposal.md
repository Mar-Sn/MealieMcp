# Proposal: OpenTelemetry Support

## Why
As the MealieMcp server interacts with external systems (Mealie API) and is intended for use by AI agents, observability is crucial.
- **Tracing:** Helps visualize the flow of requests from the MCP client -> Server -> Mealie API, making it easier to debug latency or failures.
- **Metrics:** Provides insights into usage patterns, error rates, and resource consumption.
- **Logging:** Structured logging integrated with OTel ensures logs are correlated with traces.

## What Changes
1.  **Dependencies:** Add OpenTelemetry libraries to the project (`OpenTelemetry.Extensions.Hosting`, `OpenTelemetry.Instrumentation.Http`, `OpenTelemetry.Exporter.OpenTelemetryProtocol`).
2.  **Configuration:** Update `Program.cs` to configure OpenTelemetry tracing, metrics, and logging.
3.  **Instrumentation:**
    -   Enable HTTP Client instrumentation to capture outgoing requests to Mealie.
    -   (Optional) Add a custom `ActivitySource` for `MealieMcp` to trace internal tool execution if the MCP SDK doesn't provide it automatically.
4.  **Environment Support:** Ensure standard OTel environment variables (e.g., `OTEL_EXPORTER_OTLP_ENDPOINT`) are respected.
