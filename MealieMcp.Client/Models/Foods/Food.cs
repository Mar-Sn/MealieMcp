using System.Text.Json.Serialization;

namespace MealieMcp.Client.Models.Foods;

public class Food
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("pluralName")]
    public string? PluralName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("label")]
    public FoodLabel? Label { get; set; }

    [JsonPropertyName("aliases")]
    public List<FoodAlias>? Aliases { get; set; }

    [JsonPropertyName("extras")]
    public Dictionary<string, object>? Extras { get; set; }
}

public class CreateIngredientFood
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("pluralName")]
    public string? PluralName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}