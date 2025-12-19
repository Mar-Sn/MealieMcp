# Proposal: Authentication Support

## Why
When running in hosted (SSE) mode, the MealieMcp server exposes HTTP endpoints (`/mcp/sse`, `/mcp/messages`) that allow any client with network access to invoke tools and interact with the Mealie API. This poses a security risk, especially if the server is exposed on a public or shared network.

## What Changes
1.  **Middleware:** Implement a custom ASP.NET Core middleware (or use existing authentication mechanisms) to intercept requests to the MCP endpoints.
2.  **Configuration:** Introduce a new environment variable `MCP_SERVER_API_KEY`.
3.  **Logic:**
    -   If `MCP_SERVER_API_KEY` is set, the middleware will check for a matching key in the request headers (e.g., `X-API-Key`).
    -   If the key is missing or invalid, the server will respond with `401 Unauthorized`.
    -   If `MCP_SERVER_API_KEY` is *not* set, authentication is disabled (allowing open access, e.g., for local development or trusted networks).
4.  **Scope:** This primarily applies to the SSE transport mode. Stdio mode operates over a local pipe spawned by the client, so this auth mechanism is less relevant there (though we could theoretically require it in the initial handshake if the protocol supported it, but simpler to rely on OS permissions for Stdio).

## Impact
- **Clients:** Clients connecting to the hosted server will need to be configured to send the `X-API-Key` header.
- **Stdio:** Unaffected.

## Alternatives
- **Standard Auth:** Use standard ASP.NET Core Authentication (Bearer tokens, etc.). This might be overkill for a simple tool server and harder for some CLI clients to configure easily without a full OAuth flow. API Key is a standard pattern for service-to-service auth.
