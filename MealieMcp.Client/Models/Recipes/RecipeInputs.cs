using System.Text.Json.Serialization;

namespace MealieMcp.Client.Models.Recipes;

public class CreateRecipe
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class ScrapeRecipe
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

public class RecipeInput
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("recipeIngredient")]
    public List<RecipeIngredient>? RecipeIngredient { get; set; }

    [JsonPropertyName("recipeInstructions")]
    public List<RecipeInstruction>? RecipeInstructions { get; set; }
}
