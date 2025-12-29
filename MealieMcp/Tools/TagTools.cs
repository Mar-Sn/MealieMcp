using System.ComponentModel;
using MealieMcp.Client;
using MealieMcp.Client.Models.Tags;
using MealieMcp.Mappers;
using ModelContextProtocol.Server;

namespace MealieMcp.Tools;

[McpServerToolType]
public class TagTools(IMealieClient client, ILogger<TagTools> logger)
{
    [McpServerTool(Name = "list_tags")]
    [Description("List tags with pagination")]
    public async Task<object> ListTags(
        [Description("Page number")] int? page = 1, 
        [Description("Items per page")] int? perPage = 10,
        [Description("Search query")] string? search = null)
    {
        logger.LogInformation("Listing tags page {Page}, per_page {PerPage}, search {Search}", page, perPage, search);
        var result = await client.ListTagsAsync(page ?? 1, perPage ?? 10, search);

        if (result?.Items == null) return new List<object>();

        return result.Items.Select(t => t.ToDto());
    }

    [McpServerTool(Name = "get_tag")]
    [Description("Get a tag by ID")]
    public async Task<object> GetTag(
        [Description("The ID of the tag")] string id)
    {
        logger.LogInformation("Getting tag {Id}", id);
        var t = await client.GetTagAsync(id);
        
        if (t == null) return null;
        return t.ToDto();
    }

    [McpServerTool(Name = "create_tag")]
    [Description("Create a new tag")]
    public async Task<object> CreateTag(
        [Description("The name of the tag")] string name)
    {
        logger.LogInformation("Creating new tag: {TagName}", name);
        var tag = new TagIn
        {
            Name = name
        };
        var result = await client.CreateTagAsync(tag);
        return result.ToDto(); 
    }

    [McpServerTool(Name = "update_tag")]
    [Description("Update a tag")]
    public async Task<object> UpdateTag(
        [Description("The ID of the tag to update")] string id,
        [Description("The new name of the tag")] string name)
    {
        logger.LogInformation("Updating tag {Id}", id);
        
        var update = new TagIn
        {
            Name = name
        };

        var result = await client.UpdateTagAsync(id, update);
        return result.ToDto();
    }

    [McpServerTool(Name = "delete_tag")]
    [Description("Delete a tag")]
    public async Task<string> DeleteTag(
        [Description("The ID of the tag to delete")] string id)
    {
        logger.LogInformation("Deleting tag {Id}", id);
        await client.DeleteTagAsync(id);
        return $"Tag {id} deleted successfully";
    }
}
