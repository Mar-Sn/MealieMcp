using System.Text.Json;
using System.Text.Json.Nodes;

if (args.Length < 2)
{
    Console.WriteLine("Usage: OpenApiPatcher <input-file> <output-file>");
    return 1;
}

string inputFile = args[0];
string outputFile = args[1];

if (!File.Exists(inputFile))
{
    Console.WriteLine($"Error: Input file '{inputFile}' not found.");
    return 1;
}

try
{
    string jsonString = File.ReadAllText(inputFile);
    var root = JsonNode.Parse(jsonString);

    if (root == null)
    {
        Console.WriteLine("Error: Failed to parse JSON.");
        return 1;
    }

    // Navigate to components -> schemas -> RecipeSummary -> properties
    var recipeSummaryProps = root["components"]?["schemas"]?["RecipeSummary"]?["properties"];

    if (recipeSummaryProps is JsonObject props)
    {
        Console.WriteLine("Found RecipeSummary properties. Patching...");

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
        PatchRemoveDefault(root, "RecipeTimelineEventIn", "timestamp");
        PatchRemoveDefault(root, "RecipeTimelineEventOut", "timestamp");

        // Save the modified JSON
        File.WriteAllText(outputFile, root.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        Console.WriteLine($"Successfully patched and saved to '{outputFile}'.");
        return 0;
    }
    else
    {
        Console.WriteLine("Warning: RecipeSummary properties not found in JSON. Copying original file.");
        File.Copy(inputFile, outputFile, true);
        return 0;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error processing file: {ex.Message}");
    return 1;
}

static void PatchNullableString(JsonObject props, string propertyName, string title)
{
    if (props[propertyName] is JsonObject obj)
    {
        obj.Clear();
        obj["type"] = "string";
        obj["title"] = title;
        obj["nullable"] = true;
    }
}

static void PatchRemoveDefault(JsonNode root, string schemaName, string propertyName)
{
    var props = root["components"]?["schemas"]?[schemaName]?["properties"];
    if (props is JsonObject p && p[propertyName] is JsonObject prop)
    {
        if (prop.ContainsKey("default"))
        {
            prop.Remove("default");
            Console.WriteLine($"Removed default value from {schemaName}.{propertyName}");
        }
    }
}