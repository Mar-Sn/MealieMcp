using MealieMcp.Client.Models;
using MealieMcp.Client.Models.Foods;
using MealieMcp.Client.Models.Recipes;
using MealieMcp.Client.Models.Tags;
using Refit;

namespace MealieMcp.Client;

public interface IMealieClient
{
    // Recipes
    [Get("/api/recipes")]
    Task<PaginatedResponse<RecipeSummary>> ListRecipesAsync(int page = 1, int perPage = 10);

    [Get("/api/recipes/{slug}")]
    Task<RecipeDetail> GetRecipeAsync(string slug);

    [Post("/api/recipes")]
    Task<string> CreateRecipeAsync([Body] CreateRecipe recipe);

    [Post("/api/recipes/create/url")]
    Task<string> CreateRecipeFromUrlAsync([Body] ScrapeRecipe body);

    [Patch("/api/recipes/{slug}")]
    Task<RecipeDetail> UpdateRecipeAsync(string slug, [Body] RecipeInput recipe);

    // Foods
    [Get("/api/foods")]
    Task<PaginatedResponse<Food>> ListFoodsAsync(int page = 1, int perPage = 10, string? search = null);

    [Get("/api/foods/{id}")]
    Task<Food> GetFoodAsync(string id);

    [Post("/api/foods")]
    Task<Food> CreateFoodAsync([Body] CreateIngredientFood food);

    [Put("/api/foods/{id}")]
    Task<Food> UpdateFoodAsync(string id, [Body] CreateIngredientFood food);

    [Delete("/api/foods/{id}")]
    Task DeleteFoodAsync(string id);

    // Tags
    [Get("/api/organizers/tags")]
    Task<PaginatedResponse<Tag>> ListTagsAsync(int page = 1, int perPage = 10, string? search = null);

    [Get("/api/organizers/tags/{id}")]
    Task<Tag> GetTagAsync(string id);

    [Post("/api/organizers/tags")]
    Task<Tag> CreateTagAsync([Body] TagIn tag);

    [Put("/api/organizers/tags/{id}")]
    Task<Tag> UpdateTagAsync(string id, [Body] TagIn tag);

    [Delete("/api/organizers/tags/{id}")]
    Task DeleteTagAsync(string id);
}
