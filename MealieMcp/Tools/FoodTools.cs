using System.ComponentModel;
using MealieMcp.Client;
using MealieMcp.Client.Models.Foods;
using MealieMcp.Mappers;
using ModelContextProtocol.Server;

namespace MealieMcp.Tools;

[McpServerToolType]
public class FoodTools(IMealieClient client, ILogger<FoodTools> logger)
{
    [McpServerTool(Name = "list_foods")]
    [Description("List foods (ingredients) with pagination")]
    public async Task<object> ListFoods(
        [Description("Page number")] int? page = 1, 
        [Description("Items per page")] int? perPage = 10,
        [Description("Search query")] string? search = null)
    {
        logger.LogInformation("Listing foods page {Page}, per_page {PerPage}, search {Search}", page, perPage, search);
        var result = await client.ListFoodsAsync(page ?? 1, perPage ?? 10, search);

        if (result?.Items == null) return new List<object>();
        return result.Items.Select(f => f.ToSummary());
    }

    [McpServerTool(Name = "get_food")]
    [Description("Get a food by ID")]
    public async Task<object> GetFood(
        [Description("The ID of the food")] string id)
    {
        logger.LogInformation("Getting food {Id}", id);
        var f = await client.GetFoodAsync(id) ?? throw new Exception($"Recipe with slug {id} not found");;
        return f.ToDetail();
    }

    [McpServerTool(Name = "create_food")]
    [Description("Create a new food")]
    public async Task<string> CreateFood(
        [Description("The name of the food")] string name,
        [Description("The plural name of the food")] string? pluralName = null,
        [Description("The description of the food")] string? description = null)
    {
        logger.LogInformation("Creating new food: {FoodName}", name);
        var food = new CreateIngredientFood
        {
            Name = name,
            PluralName = pluralName,
            Description = description
        };
        var result = await client.CreateFoodAsync(food);
        return result?.Id ?? string.Empty;
    }

    [McpServerTool(Name = "update_food")]
    [Description("Update a food")]
    public async Task<object> UpdateFood(
        [Description("The ID of the food to update")] string id,
        [Description("The new name of the food")] string? name = null,
        [Description("The new plural name of the food")] string? pluralName = null,
        [Description("The new description of the food")] string? description = null)
    {
        logger.LogInformation("Updating food {Id}", id);
        var existing = await client.GetFoodAsync(id);
        if (existing == null) throw new Exception($"Food with ID {id} not found");

        var update = new CreateIngredientFood
        {
            Name = name ?? existing.Name,
            PluralName = pluralName ?? existing.PluralName,
            Description = description ?? existing.Description
        };

        return await client.UpdateFoodAsync(id, update);
    }

    [McpServerTool(Name = "delete_food")]
    [Description("Delete a food")]
    public async Task<string> DeleteFood(
        [Description("The ID of the food to delete")] string id)
    {
        logger.LogInformation("Deleting food {Id}", id);
        await client.DeleteFoodAsync(id);
        return $"Food {id} deleted successfully";
    }
}
