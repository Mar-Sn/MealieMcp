using System.Text.Json.Serialization;

namespace MealieMcp.Client.Models.Tags;

public class Tag
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
}

public class TagIn
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
