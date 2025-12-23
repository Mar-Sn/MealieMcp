using System.ComponentModel;
using MealieMcp.Clients;
using MealieMcp.Clients.Models;
using ModelContextProtocol.Server;

namespace MealieMcp.Tools;

[McpServerToolType]
public class TagTools
{
    private readonly MealieClient _client;
    private readonly ILogger<TagTools> _logger;

    public TagTools(MealieClient client, ILogger<TagTools> logger)
    {
        _client = client;
        _logger = logger;
    }

    [McpServerTool(Name = "list_tags")]
    [Description("List tags with pagination")]
    public async Task<object> ListTags(
        [Description("Page number")] int? page = 1, 
        [Description("Items per page")] int? per_page = 10,
        [Description("Search query")] string? search = null)
    {
        _logger.LogInformation("Listing tags page {Page}, per_page {PerPage}, search {Search}", page, per_page, search);
        var result = await _client.Api.Organizers.Tags.GetAsync(config =>
        {
            config.QueryParameters.Page = page ?? 1;
            config.QueryParameters.PerPage = per_page ?? 10;
            if (!string.IsNullOrEmpty(search))
            {
                config.QueryParameters.Search = search;
            }
        });

        if (result?.Items == null) return new List<object>();

        return result.Items.Select(t => new
        {
            t.Id,
            t.Name,
            t.Slug
        });
    }

    [McpServerTool(Name = "get_tag")]
    [Description("Get a tag by ID")]
    public async Task<object> GetTag(
        [Description("The ID of the tag")] string id)
    {
        _logger.LogInformation("Getting tag {Id}", id);
        var t = await _client.Api.Organizers.Tags[id].GetAsync();
        
        if (t == null) return null;
        return new
        {
            t.Id,
            t.Name,
            t.Slug
        };
    }

    [McpServerTool(Name = "create_tag")]
    [Description("Create a new tag")]
    public async Task<object> CreateTag(
        [Description("The name of the tag")] string name)
    {
        _logger.LogInformation("Creating new tag: {TagName}", name);
        var tag = new TagIn
        {
            Name = name
        };
        // PostAsync returns UntypedNode according to Kiota generation in some cases, 
        // or RecipeTagResponse in others. Let's check the generated code or use UntypedNode helper if needed.
        // Based on search results: public async Task<UntypedNode> PostAsync(...)
        var result = await _client.Api.Organizers.Tags.PostAsync(tag);
        
        // Return the raw result for now as we might need to parse it if it's UntypedNode
        return result; 
    }

    [McpServerTool(Name = "update_tag")]
    [Description("Update a tag")]
    public async Task<object> UpdateTag(
        [Description("The ID of the tag to update")] string id,
        [Description("The new name of the tag")] string name)
    {
        _logger.LogInformation("Updating tag {Id}", id);
        
        var existing = await _client.Api.Organizers.Tags[id].GetAsync();
        if (existing == null) throw new Exception($"Tag with ID {id} not found");

        var update = new TagIn
        {
            Name = name
        };

        return await _client.Api.Organizers.Tags[id].PutAsync(update);
    }

    [McpServerTool(Name = "delete_tag")]
    [Description("Delete a tag")]
    public async Task<string> DeleteTag(
        [Description("The ID of the tag to delete")] string id)
    {
        _logger.LogInformation("Deleting tag {Id}", id);
        await _client.Api.Organizers.Tags[id].DeleteAsync();
        return $"Tag {id} deleted successfully";
    }
}