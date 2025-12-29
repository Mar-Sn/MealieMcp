using System.ComponentModel;
using MealieMcp.Client;
using MealieMcp.Client.Models.Recipes;
using MealieMcp.Mappers;
using ModelContextProtocol.Server;

namespace MealieMcp.Tools;

[McpServerToolType]
public class RecipeTools(IMealieClient client, ILogger<RecipeTools> logger)
{
    [McpServerTool(Name = "list_recipes")]
    [Description("List recipes with pagination")]
    public async Task<object> ListRecipes(
        [Description("Page number")] int? page = 1,
        [Description("Items per page")] int? perPage = 10)
    {
        logger.LogInformation("Listing recipes page {Page}, per_page {PerPage}", page, perPage);
        var result = await client.ListRecipesAsync(page ?? 1, perPage ?? 10);

        if (result?.Items == null) return new List<object>();

        return result.Items.Select(r => r.ToSummary());
    }

    [McpServerTool(Name = "get_recipe")]
    [Description("Get a recipe by slug")]
    public async Task<object> GetRecipe(
        [Description("The slug of the recipe")]
        string slug)
    {
        logger.LogInformation("Getting recipe {Slug}", slug);
        var r = await client.GetRecipeAsync(slug) ?? throw new Exception($"Recipe with slug {slug} not found");
        return r.ToDetail();
    }

    [McpServerTool(Name = "get_recipe_instructions")]
    [Description("Get a recipe instructions by slug")]
    public async Task<object> GetRecipeInstructions([Description("The slug of the recipe")] string slug)
    {
        logger.LogInformation("Getting recipe instructions {Slug}", slug);
        var r = await client.GetRecipeAsync(slug) ?? throw new Exception($"Recipe with slug {slug} not found");
        return r.ToDetail().RecipeInstructions ?? [];
    }

    [McpServerTool(Name = "get_recipe_ingredients")]
    [Description("Get a recipe ingredients by slug")]
    public async Task<object> GetRecipeIngredients([Description("The slug of the recipe")] string slug)
    {
        logger.LogInformation("Getting recipe ingredients {Slug}", slug);
        var r = await client.GetRecipeAsync(slug) ?? throw new Exception($"Recipe with slug {slug} not found");
        return r.ToDetail().RecipeIngredient ?? [];
    }

    [McpServerTool(Name = "create_recipe")]
    [Description("Create a new recipe")]
    public async Task<string> CreateRecipe(
        [Description("The recipe to create")] CreateRecipe recipe)
    {
        logger.LogInformation("Creating new recipe: {RecipeName}", recipe.Name);
        return await client.CreateRecipeAsync(recipe);
    }

    [McpServerTool(Name = "create_recipe_from_url")]
    [Description("Create a recipe by scraping a URL")]
    public async Task<string> CreateRecipeFromUrl(
        [Description("The URL of the recipe to scrape")]
        string url)
    {
        logger.LogInformation("Creating recipe from URL: {Url}", url);
        var body = new ScrapeRecipe { Url = url };
        return await client.CreateRecipeFromUrlAsync(body);
    }

    [McpServerTool(Name = "update_recipe")]
    [Description("Update a recipe")]
    public async Task<object> UpdateRecipe(
        [Description("The slug of the recipe to update")]
        string slug,
        [Description("The updated recipe data")]
        RecipeInput recipe)
    {
        logger.LogInformation("Updating recipe {Slug}", slug);
        return await client.UpdateRecipeAsync(slug, recipe);
    }

    [McpServerTool(Name = "update_recipe_instructions")]
    [Description("Update recipe instructions")]
    public async Task<object> UpdateRecipeInstructions(
        [Description("The slug of the recipe")]
        string slug,
        [Description("The new instructions")] List<RecipeInstruction> instructions)
    {
        logger.LogInformation("Updating instructions for recipe {Slug}", slug);
        var input = await client.GetRecipeAsync(slug)  ?? throw new Exception($"Recipe with slug {slug} not found");
        input.RecipeInstructions = instructions;
        return await client.UpdateRecipeAsync(slug, input.ToRecipeInput());
    }

    [McpServerTool(Name = "update_recipe_ingredients")]
    [Description("Update recipe ingredients")]
    public async Task<object> UpdateRecipeIngredients(
        [Description("The slug of the recipe")]
        string slug,
        [Description("The new ingredients")] List<RecipeIngredient> ingredients)
    {
        logger.LogInformation("Updating ingredients for recipe {Slug}", slug);
        var input = await client.GetRecipeAsync(slug)  ?? throw new Exception($"Recipe with slug {slug} not found");
        input.RecipeIngredient = ingredients;
        return await client.UpdateRecipeAsync(slug, input.ToRecipeInput());
    }
}