1.  **Prepare Models** <!-- id: 0 -->
    - [x] Create `MealieMcp.Client/Models/PaginatedResponse.cs`.
    - [x] Create `MealieMcp.Client/Models/Recipes/` containing: `RecipeSummary`, `Recipe` (Detail), `RecipeInstruction`, `RecipeIngredient`, `CreateRecipe`, `ScrapeRecipe`.
    - [x] Create `MealieMcp.Client/Models/Foods/` containing: `Food`, `CreateIngredientFood`.
    - [x] Create `MealieMcp.Client/Models/Tags/` containing: `Tag`.
    - [x] Ensure all models use `System.Text.Json` attributes (e.g., `[JsonPropertyName]`).
    - [x] **Discard** all other unused Kiota-generated models.

2.  **Define Refit Interface** <!-- id: 1 -->
    - [x] Create `IMealieClient.cs` in `MealieMcp.Client`.
    - [x] Define methods for **Recipes**: `ListRecipes`, `GetRecipe`, `CreateRecipe`, `CreateRecipeFromUrl`, `UpdateRecipe`.
    - [x] Define methods for **Foods**: `ListFoods`, `GetFood`, `CreateFood`, `UpdateFood`, `DeleteFood`.
    - [x] Define methods for **Tags**: `ListTags`, `GetTag`, `CreateTag`, `UpdateTag`, `DeleteTag`.
    - [x] **Exclude** any other API endpoints not currently used.

3.  **Configure Refit and Polly** <!-- id: 2 -->
    - [x] Update `MealieMcp.Client.csproj`: Remove Kiota packages, add `Refit` and `Microsoft.Extensions.Http.Polly`.
    - [x] Update `MealieMcp/Program.cs`: Remove `AddHttpClient<MealieClient>`, add `AddRefitClient<IMealieClient>` with Polly policies.

4.  **Update Tools** <!-- id: 3 -->
    - [x] Update `RecipeTools.cs`: Inject `IMealieClient`, replace calls.
    - [x] Update `FoodTools.cs`: Inject `IMealieClient`, replace calls.
    - [x] Update `TagTools.cs`: Inject `IMealieClient`, replace calls.

5.  **Cleanup Build** <!-- id: 4 -->
    - [x] Remove `Build/GenerateClientTask.cs`.
    - [x] Update `Build/Build.csproj` to remove task reference.
    - [x] Update `Build/Program.cs` to remove task execution.
