# Tasks: MCP Actions for Recipe Management

1.  **Dependency Setup**
    -   [ ] Add `Microsoft.Extensions.Http` package to `MealieMcp.csproj`. <!-- id: 0 -->
    -   [ ] Add `NSwag.ApiDescription.Client` package to `MealieMcp.csproj`. <!-- id: 1 -->

2.  **Client Generation**
    -   [ ] Configure NSwag (via `OpenApiReference` in csproj or `nswag.json`) to generate the C# client from `openapi.json` into a `MealieClient.cs` file (and DTOs). <!-- id: 12 -->
    -   [ ] Run the build to generate the client and verify `MealieClient.cs` is created. <!-- id: 13 -->

3.  **Configuration**
    -   [ ] Implement logic to read `MEALIE_API_URL` and `MEALIE_API_TOKEN` environment variables. <!-- id: 2 -->
    -   [ ] Register the generated `MealieClient` (interface and implementation) with `IHttpClientFactory` in `Program.cs`, configuring the base address and authorization header. <!-- id: 3 -->

4.  **Tool Implementation**
    -   [ ] Implement `list_recipes` tool in `Program.cs` using `MealieClient` and register it. <!-- id: 5 -->
    -   [ ] Implement `get_recipe` tool using `MealieClient` and register it. <!-- id: 6 -->
    -   [ ] Implement `create_recipe` tool using `MealieClient` and register it. <!-- id: 7 -->
    -   [ ] Implement `create_recipe_from_url` tool using `MealieClient` and register it. <!-- id: 8 -->
    -   [ ] Implement `update_recipe` tool using `MealieClient` and register it. <!-- id: 9 -->

5.  **Validation**
    -   [ ] Verify the server builds and runs. <!-- id: 10 -->
    -   [ ] Verify tools appear in the MCP client (if testable locally). <!-- id: 11 -->
