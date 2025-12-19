# Tasks: SSE Support

1.  **Project Configuration**
    -   [x] Update `MealieMcp.csproj` SDK to `Microsoft.NET.Sdk.Web`. <!-- id: 0 -->
    -   [x] Update `Dockerfile` to use `mcr.microsoft.com/dotnet/aspnet:10.0` as the runtime base image (instead of just `runtime`). <!-- id: 1 -->

2.  **Implementation**
    -   [x] Refactor `Program.cs` to use `WebApplication.CreateBuilder`. <!-- id: 2 -->
    -   [x] Implement logic in `Program.cs` to choose transport based on environment variable `MCP_TRANSPORT` (defaulting to `sse` or `stdio` as appropriate, or strictly `sse` for this pass). *Refinement: Default to SSE, but keep Stdio if `--stdio` arg is present or specific env var is set.* <!-- id: 3 -->
    -   [x] Configure SSE endpoints (`/mcp/sse`, `/mcp/message`) in `Program.cs`. <!-- id: 4 -->

3.  **Validation**
    -   [x] Verify `stdio` mode still works (local test with `dotnet run -- --stdio`). <!-- id: 5 -->
    -   [x] Verify `sse` mode works (start server, connect via MCP inspector or curl). <!-- id: 6 -->
    -   [x] Verify Docker container builds and runs. <!-- id: 7 -->
