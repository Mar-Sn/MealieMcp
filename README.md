# MealieMcp 🍲🤖

**MealieMcp** is a [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) server that bridges the gap between AI agents and your self-hosted [Mealie](https://mealie.io/) instance.

It empowers LLMs (like Claude, ChatGPT, etc.) to interact directly with your recipe database, allowing them to search for recipes, manage ingredients, organize tags, and more—all through a standardized interface.

## ✨ Features

- **Recipe Management:** Search, retrieve, and manage your recipe collection.
- **Food & Ingredient Control:** Manage the core food database within Mealie.
- **Tag Organization:** Create and assign tags to keep your recipes organized.
- **Dual Transport Support:** Runs via `stdio` (for local desktop apps like Claude Desktop) or `HTTP/SSE` (for remote/containerized deployments).
- **Resilient & Observable:** Built with .NET 9, featuring OpenTelemetry for tracing/metrics and Polly for network resilience.

## 🚀 Getting Started

### Prerequisites

- A running **Mealie** instance (v1.0.0+ recommended).
- A **Mealie API Token** (User Settings -> API Tokens).

### 🐳 Run with Docker (Recommended)

You can easily run MealieMcp as a Docker container.

```bash
docker run -d \
  --name mealie-mcp \
  -e MEALIE_API_URL="http://localhost:9000" \
  -e MCP_TRANSPORT="sse" \
  -p 8080:8080 \
  ghcr.io/mealie-recipes/mealie:latest
```

### 💻 Local Development

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/yourusername/MealieMcp.git
    cd MealieMcp
    ```

2.  **Configure Environment:**
    Set the following environment variables (or use `appsettings.Development.json`):
    -   `MEALIE_API_URL`: Your Mealie URL (e.g., `http://localhost:9000`)
    -   `MEALIE_API_TOKEN`: Your API Token

3.  **Run the Server:**
    ```bash
    cd MealieMcp
    dotnet run -- --stdio
    ```

## ⚙️ Configuration

| Variable             | Description                                                | Default                             |
|:---------------------|:-----------------------------------------------------------|:------------------------------------|
| `MEALIE_API_URL`     | Base URL of your Mealie instance                           | **Required**                        |
| `MEALIE_API_TOKEN`   | Your Mealie API Token                                      | **Required**                        |
| `MCP_TRANSPORT`      | Transport mode: `stdio` or `http`                          | `http` (unless `--stdio` arg used)  |
| `MCP_SERVER_API_KEY` | Optional API Key to secure the MCP server (HTTP mode only) | `null`                              |

## 🔌 Connecting to MCP Clients

### Claude Desktop (Stdio Mode)

To use MealieMcp with Claude Desktop, add the following to your `claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "mealie": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "/path/to/MealieMcp/MealieMcp/MealieMcp.csproj",
        "--",
        "--stdio"
      ],
      "env": {
        "MEALIE_API_URL": "http://localhost:9000",
        "MEALIE_API_TOKEN": "your-super-secret-token"
      }
    }
  }
}
```

*Note: Make sure you have the .NET SDK installed if running from source.*

## 🛠️ Development

This project follows Clean Code principles and uses the latest .NET features.

-   **Build:** `dotnet build`
-   **Test:** `dotnet test` (includes E2E tests in `MealieMcp.E2E`)
-   **Architecture:**
    -   `MealieMcp`: Main MCP Server implementation.
    -   `MealieMcp.Client`: Refit-based API client for Mealie.
    -   `MealieMcp.AppHost`: .NET Aspire orchestration.

## 📄 License

[MIT](LICENSE)
