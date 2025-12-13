# Design: Recipe Management via MCP

## Architecture

### HTTP Client Generation
We will use **NSwag** to generate a typed C# HTTP client from the provided `openapi.json` file. This ensures our data models and API calls strictly adhere to the Mealie API specification and reduces manual maintenance.
- **Client Generation**: A build step or manual command will generate the `MealieClient` class and associated DTOs.
- **Base URL & Token**: The generated client will be configured via `IHttpClientFactory` in `Program.cs`, injecting the base URL and authorization headers from environment variables.
- **Serialization**: The generated client will use `System.Text.Json` (or `Newtonsoft.Json` depending on NSwag configuration, aiming for `System.Text.Json` for consistency).

### Tool Mapping
We will map the generated Mealie API client methods to MCP tools. The mapping logic remains similar, but the implementation will call the generated methods:

| MCP Tool | Mealie Endpoint | Generated Method (Approx) | Notes |
| :--- | :--- | :--- | :--- |
| `list_recipes` | `/api/recipes` | `GetAllRecipesAsync` | Supports pagination and basic filtering. |
| `get_recipe` | `/api/recipes/{slug}` | `GetRecipeAsync` | Returns full recipe details. |
| `create_recipe` | `/api/recipes` | `CreateRecipeAsync` | Creates a recipe from structured JSON. |
| `create_recipe_from_url` | `/api/recipes/create/url` | `CreateRecipeFromUrlAsync` | Imports from a URL. |
| `update_recipe` | `/api/recipes/{slug}` | `UpdateRecipeAsync` | Updates an existing recipe. |

### Error Handling
The generated client typically throws `ApiException` (or similar) for non-success status codes. We will wrap these calls in the MCP tool handlers to catch these exceptions and return formatted error messages to the AI.

## Dependencies
- `NSwag.ApiDescription.Client` (or similar NSwag package) for generation.
- `Microsoft.Extensions.Http` for `IHttpClientFactory`.
