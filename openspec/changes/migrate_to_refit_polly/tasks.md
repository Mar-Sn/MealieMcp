1.  **Prepare Models** <!-- id: 0 -->
    - Create `MealieMcp.Client/Models/PaginatedResponse.cs`.
    - Create `MealieMcp.Client/Models/Recipes/` containing: `RecipeSummary`, `Recipe` (Detail), `RecipeInstruction`, `RecipeIngredient`, `CreateRecipe`, `ScrapeRecipe`.
    - Create `MealieMcp.Client/Models/Foods/` containing: `Food`, `CreateIngredientFood`.
    - Create `MealieMcp.Client/Models/Tags/` containing: `Tag`.
    - Ensure all models use `System.Text.Json` attributes (e.g., `[JsonPropertyName]`).
    - **Discard** all other unused Kiota-generated models.

2.  **Define Refit Interface** <!-- id: 1 -->
    - Create `IMealieClient.cs` in `MealieMcp.Client`.
    - Define methods for **Recipes**: `ListRecipes`, `GetRecipe`, `CreateRecipe`, `CreateRecipeFromUrl`, `UpdateRecipe`.
    - Define methods for **Foods**: `ListFoods`, `GetFood`, `CreateFood`, `UpdateFood`, `DeleteFood`.
    - Define methods for **Tags**: `ListTags`, `GetTag`, `CreateTag`, `UpdateTag`, `DeleteTag`.
    - **Exclude** any other API endpoints not currently used.

3.  **Configure Refit and Polly** <!-- id: 2 -->
    - Update `MealieMcp.Client.csproj`: Remove Kiota packages, add `Refit` and `Microsoft.Extensions.Http.Polly`.
    - Update `MealieMcp/Program.cs`: Remove `AddHttpClient<MealieClient>`, add `AddRefitClient<IMealieClient>` with Polly policies.

4.  **Update Tools** <!-- id: 3 -->
    - Update `RecipeTools.cs`: Inject `IMealieClient`, replace calls.
    - Update `FoodTools.cs`: Inject `IMealieClient`, replace calls.
    - Update `TagTools.cs`: Inject `IMealieClient`, replace calls.

5.  **Cleanup Build** <!-- id: 4 -->
    - Remove `Build/GenerateClientTask.cs`.
    - Update `Build/Build.csproj` to remove task reference.
    - Update `Build/Program.cs` to remove task execution.
