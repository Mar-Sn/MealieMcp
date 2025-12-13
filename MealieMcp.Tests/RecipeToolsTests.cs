using Moq;
using MealieMcp.Tools;
using MealieMcp.Clients;
using Microsoft.Extensions.Logging;

namespace MealieMcp.Tests;

public class RecipeToolsTests
{
    [Fact]
    public async Task CreateRecipeFromUrl_Calls_Client()
    {
        // Arrange
        var mockHttpClient = new HttpClient();
        // Construct mock with required constructor args for MealieClient
        var mockClient = new Mock<MealieClient>("http://localhost", mockHttpClient) { CallBase = true };
        var mockLogger = new Mock<ILogger<RecipeTools>>();

        // Setup expected behavior
        // Note: AcceptLanguage106 might vary if spec changes/regenerates. 
        // Using It.IsAny with explicit type found in generation.
        mockClient.Setup(c => c.Parse_recipe_url_api_recipes_create_url_postAsync(
            It.IsAny<AcceptLanguage106>(), 
            It.IsAny<ScrapeRecipe>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync("test-slug");

        var tools = new RecipeTools(mockClient.Object, mockLogger.Object);

        // Act
        var result = await tools.CreateRecipeFromUrl("http://example.com/recipe");

        // Assert
        Assert.Equal("test-slug", result);
        
        mockClient.Verify(c => c.Parse_recipe_url_api_recipes_create_url_postAsync(
            null, 
            It.Is<ScrapeRecipe>(s => s.Url == "http://example.com/recipe"), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
