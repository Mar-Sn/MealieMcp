using System.Text.Json;
using System.Text.Json.Nodes;

[TaskName("Patch-OpenApi")]
public class PatchOpenApiTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Log.Information("Patching OpenAPI spec...");
        
        var inputFile = context.OpenApiJson;

        if (!File.Exists(inputFile))
        {
            throw new FileNotFoundException($"Input file '{inputFile}' not found.");
        }

        try
        {
            var jsonString = File.ReadAllText(inputFile);
            var root = JsonNode.Parse(jsonString);

            if (root == null)
            {
                throw new Exception("Error: Failed to parse JSON.");
            }

            // Navigate to components -> schemas -> RecipeSummary -> properties
            var recipeSummaryProps = root["components"]?["schemas"]?["RecipeSummary"]?["properties"];

            if (recipeSummaryProps is JsonObject props)
            {
                context.Log.Information("Found RecipeSummary properties. Patching...");

                // Patch 'id'
                if (props["id"] is JsonObject idObj)
                {
                     // Clear existing properties (like "anyOf")
                    idObj.Clear();
                    // Set simple string definition
                    idObj["type"] = "string";
                    idObj["format"] = "uuid4";
                    idObj["title"] = "Id";
                    idObj["nullable"] = true;
                }

                // Patch other nullable string fields
                PatchNullableString(props, "name", "Name");
                PatchNullableString(props, "recipeYield", "Recipe Yield");
                PatchNullableString(props, "totalTime", "Total Time");
                PatchNullableString(props, "prepTime", "Prep Time");
                PatchNullableString(props, "cookTime", "Cook Time");
                PatchNullableString(props, "performTime", "Perform Time");
                PatchNullableString(props, "description", "Description");
                PatchNullableString(props, "rating", "Rating");
                PatchNullableString(props, "orgURL", "Org URL");

                // Patch RecipeTimelineEventIn and RecipeTimelineEventOut defaults
                PatchRemoveDefault(context, root, "RecipeTimelineEventIn", "timestamp");
                PatchRemoveDefault(context, root, "RecipeTimelineEventOut", "timestamp");

                // Save the modified JSON
                File.WriteAllText(context.ProcessedOpenApiSpecFile, root.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                context.Log.Information($"Successfully patched and saved to '{context.ProcessedOpenApiSpecFile}'.");
            }
            else
            {
                context.Log.Warning("RecipeSummary properties not found in JSON. Copying original file.");
                File.Copy(inputFile, context.ProcessedOpenApiSpecFile, true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error processing file: {ex.Message}", ex);
        }
    }

    private void PatchNullableString(JsonObject props, string propertyName, string title)
    {
        if (props[propertyName] is JsonObject obj)
        {
            obj.Clear();
            obj["type"] = "string";
            obj["title"] = title;
            obj["nullable"] = true;
        }
    }

    private void PatchRemoveDefault(BuildContext context, JsonNode root, string schemaName, string propertyName)
    {
        var props = root["components"]?["schemas"]?[schemaName]?["properties"];
        if (props is JsonObject p && p[propertyName] is JsonObject prop)
        {
            if (prop.ContainsKey("default"))
            {
                prop.Remove("default");
                context.Log.Information($"Removed default value from {schemaName}.{propertyName}");
            }
        }
    }
}