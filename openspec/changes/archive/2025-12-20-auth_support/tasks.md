# Tasks: Authentication Support

1.  **Implementation**
    -   [X] Implement `ApiKeyAuthMiddleware` class in `MealieMcp/Middleware/ApiKeyAuthMiddleware.cs`. <!-- id: 0 -->
    -   [X] Update `Program.cs` to read `MCP_SERVER_API_KEY` from configuration. <!-- id: 1 -->
    -   [X] Update `Program.cs` to register the middleware *before* the MCP endpoints are mapped, but only if the API key is configured. <!-- id: 2 -->

2.  **Validation**
    -   [X] Verify the server starts without an API key (auth disabled). <!-- id: 3 -->
    -   [X] Verify the server rejects requests without a key when `MCP_SERVER_API_KEY` is set. <!-- id: 4 -->
    -   [X] Verify the server accepts requests with the correct `X-API-Key` header when `MCP_SERVER_API_KEY` is set. <!-- id: 5 -->
