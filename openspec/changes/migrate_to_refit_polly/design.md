# Design: Refit and Polly Migration

## Architecture
- **Client Interface**: `IMealieClient` (Refit interface) will replace `MealieClient` (Kiota concrete class).
- **Dependency Injection**: `IMealieClient` will be registered using `AddRefitClient` in `Program.cs`.
- **Resilience**: Polly policies will be added to the `HttpClient` builder returned by `AddRefitClient`.
- **Models**: Standard C# POCOs (Records or Classes) in `MealieMcp.Client.Models` (or similar namespace).

## Polly Policies
- **Retry**: Exponential backoff (e.g., 3 retries) for transient errors (5xx, 408).
- **Circuit Breaker**: Break after a certain number of consecutive failures to prevent cascading failures.

## Mapping
- Current Kiota calls: `_client.Api.Recipes.GetAsync(...)`
- New Refit calls: `_client.GetRecipesAsync(...)`

## Impact Analysis
- **Build System**: `GenerateClientTask` will be removed.
- **Dependencies**: Remove `Microsoft.Kiota.*`, Add `Refit`, `Refit.HttpClientFactory`, `Microsoft.Extensions.Http.Polly`.
