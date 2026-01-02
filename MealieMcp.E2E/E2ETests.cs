using System.Text.Json.Nodes;
using Aspire.Hosting.Testing;
using MealieMcp.Client.Models.Recipes;

namespace MealieMcp.E2E;

[CollectionDefinition("McpServer")]
public class McpServerCollection : ICollectionFixture<McpServerFixture>
{
}

[Collection("McpServer")]
public class E2ETests(McpServerFixture fixture)
{
    [Fact]
    public async Task ToolDiscovery()
    {
        var mcpClient = new McpClient(fixture.App.CreateHttpClient("mealiemcp"));
        await mcpClient.InitializeAsync("mcp/sse");

        var result = await mcpClient.PostMessageAsync("tools/list");

        await VerifyJson(result![0]!.AsArray().ToJsonString())
            .UseDirectory("Snapshots");
    }

    [Fact]
    public async Task CreateAndEditRecipe()
    {
        var mcpClient = new McpClient(fixture.App.CreateHttpClient("mealiemcp"));
        await mcpClient.InitializeAsync("mcp/sse");

        // 1. Create Recipe
        JsonNode? createResult;
        try
        {
            createResult = await mcpClient.CallToolAsync("create_recipe", new { name = "Test Recipe" });
        }
        catch (Exception ex)
        {
            throw new Exception($"Create recipe call failed with exception: {ex.Message}", ex);
        }

        var content = createResult?["content"]?[0];
        var slug = content?["text"]?.GetValue<string>().Replace("\"", string.Empty);

        Assert.NotNull(slug);
        Assert.False(string.IsNullOrEmpty(slug));
        if (createResult?["isError"]?.GetValue<bool>() == true)
        {
            throw new Exception($"Create recipe failed: {createResult?["content"]?[0]?["text"]?.GetValue<string>()}");
        }

        createResult = await mcpClient.CallToolAsync("create_food", new { name = "Bread" });
        content = createResult?["content"]?[0];
        var foodId = content?["text"]?.GetValue<string>();

        // 2. Add Ingredients
        var ingredientParams = new
        {
            slug,
            ingredients = new RecipeIngredient[]
            {
                new()
                {
                    Food = new RecipeIngredientFood()
                    {
                        Id = foodId,
                        Name = "Bread",
                    },
                    Display = "1 loaf",
                    Quantity = 1,
                    ReferenceId = Guid.NewGuid().ToString()
                }
            }
        };

        var updateResult = await mcpClient.CallToolAsync("update_recipe_ingredients", ingredientParams);

        if (updateResult?["isError"]?.GetValue<bool>() == true)
        {
            throw new Exception(
                $"Update ingredients failed: {updateResult?["content"]?[0]?["text"]?.GetValue<string>()}");
        }

        // 3. Add Instructions

        var instructionParams = new
        {
            slug,
            instructions = new RecipeInstruction[]
            {
                new() { Summary = "Mix ingredients", Text = "Mix all ingredients together" },
                new() { Summary = "Bake at 350F", Text = "Bake for 30 minutes" }
            }
        };

        var instructionResult = await mcpClient.CallToolAsync("update_recipe_instructions", instructionParams);

        if (instructionResult?["isError"]?.GetValue<bool>() == true)
        {
            throw new Exception(
                $"Update instructions failed: {instructionResult?["content"]?[0]?["text"]?.GetValue<string>()}");
        }

        // 4. Verify Final State by getting recipe
        var getResult = await mcpClient.CallToolAsync("get_recipe", new { slug = slug });
        
        var recipeJson = getResult?["content"]?[0]?["text"]?.GetValue<string>();

        await VerifyJson(recipeJson)
            .UseDirectory("Snapshots")
            .IgnoreMember("dateAdded")
            .ScrubGuids()
            .IgnoreMember("slug");
    }
}