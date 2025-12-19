# Proposal: SSE (Server-Sent Events) Support

## Why
Currently, MealieMcp uses the `stdio` transport, which requires the client (e.g., Gemini CLI) to spawn and manage the server process. This has limitations:
- **Persistence:** The server shuts down when the client disconnects.
- **Scalability:** It's difficult to run the server in a containerized environment (like Kubernetes or Docker Compose) and have external clients connect to it.
- **Accessibility:** Only local clients can easily connect.

Moving to (or adding) **SSE (Server-Sent Events)** transport allows MealieMcp to run as a standard HTTP web server. This enables:
- **Permanent Hosting:** The server can run indefinitely in a container.
- **Remote Access:** Clients can connect via HTTP.
- **Decoupling:** The server lifecycle is independent of the client.

## What Changes

1.  **Project SDK:** Update `MealieMcp.csproj` to use `Microsoft.NET.Sdk.Web` to support ASP.NET Core features.
2.  **Transport Configuration:**
    -   Modify `Program.cs` to use `WebApplication` instead of `Host`.
    -   Configure the MCP server to use `WithSseServerTransport("/mcp/sse", "/mcp/message")` when running in hosted mode.
    -   (Optional) Retain `stdio` support via a command-line flag or environment variable (e.g., `MCP_TRANSPORT=stdio` vs `sse`), or simply default to SSE for the web project. *Decision: We will support SSE as the primary mode for the web-enabled build, but allow `stdio` if args are passed, or via configuration.*
3.  **Docker:** The existing `Dockerfile` already exposes port 8080. We need to ensure the application listens on it and the startup command aligns with the web host.

## Impact
- **Breaking Change:** If we strictly switch to `Microsoft.NET.Sdk.Web` and change the entry point logic significantly, existing `stdio` clients might need reconfiguration (though we can maintain backward compatibility by checking args).
- **New Dependency:** ASP.NET Core runtime (already in `mcr.microsoft.com/dotnet/runtime:10.0`? No, usually requires `aspnet` runtime image for the final stage).

## Alternatives
- **Stdio over TCP:** Custom wrappers, but SSE is the standard MCP HTTP transport.
