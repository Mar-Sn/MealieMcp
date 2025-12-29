using System.Text.Json.Serialization;

namespace MealieMcp.Client.Models.Foods;

public class FoodLabel
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }
}

public class FoodAlias
{
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }
}
