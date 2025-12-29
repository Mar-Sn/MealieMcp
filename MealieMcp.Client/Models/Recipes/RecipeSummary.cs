using System.Text.Json.Serialization;

namespace MealieMcp.Client.Models.Recipes;

public class RecipeSummary
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("dateAdded")]
    public DateTime? DateAdded { get; set; }

    [JsonPropertyName("dateUpdated")]
    public DateTimeOffset? DateUpdated { get; set; }

    [JsonPropertyName("rating")]
    public double? Rating { get; set; }

    [JsonPropertyName("recipeServings")]
    public double? RecipeServings { get; set; }

    [JsonPropertyName("recipeYieldQuantity")]
    public double? RecipeYieldQuantity { get; set; }
}