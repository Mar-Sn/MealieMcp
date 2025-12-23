using System.ComponentModel;
using MealieMcp.Clients;
using MealieMcp.Clients.Models;
using ModelContextProtocol.Server;

namespace MealieMcp.Tools;

[McpServerToolType]
public class FoodTools
{
    private readonly MealieClient _client;
    private readonly ILogger<FoodTools> _logger;

    public FoodTools(MealieClient client, ILogger<FoodTools> logger)
    {
        _client = client;
        _logger = logger;
    }

    [McpServerTool(Name = "list_foods")]
    [Description("List foods (ingredients) with pagination")]
    public async Task<object> ListFoods(
        [Description("Page number")] int? page = 1, 
        [Description("Items per page")] int? per_page = 10,
        [Description("Search query")] string? search = null)
    {
        _logger.LogInformation("Listing foods page {Page}, per_page {PerPage}, search {Search}", page, per_page, search);
        var result = await _client.Api.Foods.GetAsync(config =>
        {
            config.QueryParameters.Page = page ?? 1;
            config.QueryParameters.PerPage = per_page ?? 10;
            if (!string.IsNullOrEmpty(search))
            {
                config.QueryParameters.Search = search;
            }
        });

        if (result?.Items == null) return new List<object>();

        return result.Items.Select(f => new
        {
            f.Id,
            f.Name,
            f.Description,
            f.Label
        });
    }

    [McpServerTool(Name = "get_food")]
    [Description("Get a food by ID")]
    public async Task<object> GetFood(
        [Description("The ID of the food")] string id)
    {
        _logger.LogInformation("Getting food {Id}", id);
        var f = await _client.Api.Foods[id].GetAsync();
        
        if (f == null) return null;
        return new
        {
            f.Id,
            f.Name,
            f.Description,
            f.Aliases,
            f.Label,
            f.Extras
        };
    }

    [McpServerTool(Name = "create_food")]
    [Description("Create a new food")]
    public async Task<string> CreateFood(
        [Description("The name of the food")] string name,
        [Description("The description of the food")] string? description = null)
    {
        _logger.LogInformation("Creating new food: {FoodName}", name);
        var food = new CreateIngredientFood
        {
            Name = name,
            Description = description
        };
        var result = await _client.Api.Foods.PostAsync(food);
        return result?.Id ?? string.Empty;
    }

    [McpServerTool(Name = "update_food")]
    [Description("Update a food")]
    public async Task<object> UpdateFood(
        [Description("The ID of the food to update")] string id,
        [Description("The new name of the food")] string? name = null,
        [Description("The new description of the food")] string? description = null)
    {
        _logger.LogInformation("Updating food {Id}", id);
        
        var existing = await _client.Api.Foods[id].GetAsync();
        if (existing == null) throw new Exception($"Food with ID {id} not found");

        var update = new CreateIngredientFood
        {
            Name = name ?? existing.Name,
            Description = description ?? existing.Description
        };

        return await _client.Api.Foods[id].PutAsync(update);
    }

    [McpServerTool(Name = "delete_food")]
    [Description("Delete a food")]
    public async Task<string> DeleteFood(
        [Description("The ID of the food to delete")] string id)
    {
        _logger.LogInformation("Deleting food {Id}", id);
        await _client.Api.Foods[id].DeleteAsync();
        return $"Food {id} deleted successfully";
    }
}