using System.ComponentModel;
using MealieMcp.Clients;
using MealieMcp.Clients.Models;
using MealieMcp.Mappers;
using ModelContextProtocol.Server;

namespace MealieMcp.Tools;

[McpServerToolType]
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
        var result = await _client.Api.Recipes.GetAsync(config =>
        {
            config.QueryParameters.Page = page ?? 1;
            config.QueryParameters.PerPage = per_page ?? 10;
        });

        if (result?.Items == null) return new List<object>();

        return result.Items.Select(r => r.ToSummary());
    }

    [McpServerTool(Name = "get_recipe")]
    [Description("Get a recipe by slug")]
    public async Task<object> GetRecipe(
        [Description("The slug of the recipe")] string slug)
    {
        _logger.LogInformation("Getting recipe {Slug}", slug);
        var r = await _client.Api.Recipes[slug].GetAsync();
        
        if (r == null) return null;
        return r.ToDetail();
    }

    [McpServerTool(Name = "create_recipe")]
    [Description("Create a new recipe")]
    public async Task<string> CreateRecipe(
        [Description("The recipe to create")] CreateRecipe recipe)
    {
        _logger.LogInformation("Creating new recipe: {RecipeName}", recipe.Name);
        return await _client.Api.Recipes.PostAsync(recipe);
    }

    [McpServerTool(Name = "create_recipe_from_url")]
    [Description("Create a recipe by scraping a URL")]
    public async Task<string> CreateRecipeFromUrl(
        [Description("The URL of the recipe to scrape")] string url)
    {
        _logger.LogInformation("Creating recipe from URL: {Url}", url);
        var body = new ScrapeRecipe { Url = url };
        return await _client.Api.Recipes.Create.Url.PostAsync(body);
    }

    [McpServerTool(Name = "update_recipe")]
    [Description("Update a recipe")]
    public async Task<object> UpdateRecipe(
        [Description("The slug of the recipe to update")] string slug,
        [Description("The updated recipe data")] RecipeInput recipe)
    {
        _logger.LogInformation("Updating recipe {Slug}", slug);
        try
        {
            return await _client.Api.Recipes[slug].PutAsync(recipe);
        }
        catch (Exception ex)
        {
            return new { error = ex.ToString() };
        }
    }
}