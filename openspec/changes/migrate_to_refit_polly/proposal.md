# Change: Migrate to Refit and Polly

## Why
The current Kiota-generated client is verbose and difficult to maintain. More critically, Kiota struggles to generate correct models due to the complexity of the OpenAPI specifications, often resulting in overly complex wrapper types. Migrating to Refit allows for the use of simple, clean models (e.g., plain strings instead of complex generated wrappers) and simplifies the codebase. Additionally, adding Polly will improve robustness against transient network failures.

## What Changes
- Replace the Kiota-generated `MealieClient` with a Refit interface `IMealieClient`.
- Remove Kiota generation build tasks and dependencies.
- Add Refit and Polly dependencies.
- Implement retry and circuit breaker policies for API requests.
- Update `RecipeTools`, `FoodTools`, and `TagTools` to use the new Refit client.
- **Strict Scoping**: Migrate *only* the models and endpoints strictly required by the current tools (`RecipeTools`, `FoodTools`, `TagTools`). All other Kiota-generated models and API wrappers will be removed and not migrated.

### Migrated Components

**API Endpoints (Refit Interface):**
- **Recipes**:
    - `GET /api/recipes` (List)
    - `GET /api/recipes/{slug}` (Get)
    - `POST /api/recipes` (Create)
    - `POST /api/recipes/create/url` (Scrape)
    - `PATCH /api/recipes/{slug}` (Update)
- **Foods**:
    - `GET /api/foods` (List)
    - `GET /api/foods/{id}` (Get)
    - `POST /api/foods` (Create)
    - `PUT /api/foods/{id}` (Update)
    - `DELETE /api/foods/{id}` (Delete)
- **Tags**:
    - `GET /api/organizers/tags` (List)
    - `GET /api/organizers/tags/{id}` (Get)
    - `POST /api/organizers/tags` (Create)
    - `PUT /api/organizers/tags/{id}` (Update)
    - `DELETE /api/organizers/tags/{id}` (Delete)

**Models (POCOs):**
- **Shared**: `PaginatedResponse<T>`
- **Recipes**: `RecipeSummary`, `Recipe` (Detail), `RecipeInstruction`, `RecipeIngredient`, `CreateRecipe`, `ScrapeRecipe`
- **Foods**: `Food`, `CreateIngredientFood`
- **Tags**: `Tag`

## Impact
- **Affected specs**: `refit_client` (new capability)
- **Affected code**: `MealieMcp.Client` project, `MealieMcp/Tools/*.cs`, `MealieMcp/Program.cs`, `Build` project.
- **Breaking Changes**: Internal implementation details change, but external MCP tool interface remains compatible.
