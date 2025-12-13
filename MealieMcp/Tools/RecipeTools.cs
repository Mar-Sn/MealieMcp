using System.ComponentModel;
using MealieMcp.Clients;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace MealieMcp.Tools;

public class RecipeTools
{
    private readonly MealieClient _client;
    private readonly ILogger<RecipeTools> _logger;

    public RecipeTools(MealieClient client, ILogger<RecipeTools> logger)
    {
        _client = client;
        _logger = logger;
    }

    [McpServerTool(Name = "list_recipes")]
    [Description("List recipes with pagination")]
    public async Task<object> ListRecipes(
        [Description("Page number")] int? page = 1, 
        [Description("Items per page")] int? per_page = 10)
    {
        _logger.LogInformation("Listing recipes page {Page}, per_page {PerPage}", page, per_page);
        var result = await _client.Get_all_api_recipes_getAsync(
            null, // categories
            null, // tags
            null, // tools
            null, // foods
            null, // households
            null, // orderBy
            null, // orderByNullPosition
            null, // orderDirection
            null, // queryFilter
            null, // paginationSeed
            page ?? 1, 
            per_page ?? 10,
            null, // cookbook
            null, // requireAllCategories
            null, // requireAllTags
            null, // requireAllTools
            null, // requireAllFoods
            null, // search
            null  // accept_language
        );
        return result.Items;
    }

    [McpServerTool(Name = "get_recipe")]
    [Description("Get a recipe by slug")]
    public async Task<object> GetRecipe(
        [Description("The slug of the recipe")] string slug)
    {
        _logger.LogInformation("Getting recipe {Slug}", slug);
        return await _client.Get_one_api_recipes__slug__getAsync(slug, null);
    }

    [McpServerTool(Name = "create_recipe")]
    [Description("Create a new recipe")]
    public async Task<string> CreateRecipe(
        [Description("The recipe to create")] CreateRecipe recipe)
    {
        _logger.LogInformation("Creating new recipe: {RecipeName}", recipe.Name);
        return await _client.Create_one_api_recipes_postAsync(null, recipe);
    }

    [McpServerTool(Name = "create_recipe_from_url")]
    [Description("Create a recipe by scraping a URL")]
    public async Task<string> CreateRecipeFromUrl(
        [Description("The URL of the recipe to scrape")] string url)
    {
        _logger.LogInformation("Creating recipe from URL: {Url}", url);
        var body = new ScrapeRecipe { Url = url };
        return await _client.Parse_recipe_url_api_recipes_create_url_postAsync(null, body);
    }

    [McpServerTool(Name = "update_recipe")]
    [Description("Update a recipe")]
    public async Task<object> UpdateRecipe(
        [Description("The slug of the recipe to update")] string slug,
        [Description("The updated recipe data")] RecipeInput recipe)
    {
        _logger.LogInformation("Updating recipe {Slug}", slug);
        return await _client.Update_one_api_recipes__slug__putAsync(slug, null, recipe);
    }
}