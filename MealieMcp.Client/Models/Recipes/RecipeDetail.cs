using System.Text.Json.Serialization;

namespace MealieMcp.Client.Models.Recipes;

public class RecipeDetail : RecipeSummary
{
    [JsonPropertyName("recipeCategory")]
    public List<RecipeCategory>? RecipeCategory { get; set; }

    [JsonPropertyName("tags")]
    public List<RecipeTag>? Tags { get; set; }

    [JsonPropertyName("tools")]
    public List<RecipeTool>? Tools { get; set; }

    [JsonPropertyName("recipeIngredient")]
    public List<RecipeIngredient>? RecipeIngredient { get; set; }

    [JsonPropertyName("recipeInstructions")]
    public List<RecipeInstruction>? RecipeInstructions { get; set; }

    [JsonPropertyName("cookTime")]
    public string? CookTime { get; set; }

    [JsonPropertyName("prepTime")]
    public string? PrepTime { get; set; }

    [JsonPropertyName("totalTime")]
    public string? TotalTime { get; set; }

    [JsonPropertyName("performTime")]
    public string? PerformTime { get; set; }
}

public class RecipeCategory
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
}

public class RecipeTag
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
}

public class RecipeTool
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
}

public class RecipeIngredient
{
    [JsonPropertyName("display")]
    public string? Display { get; set; }
    
    [JsonPropertyName("food")]
    public RecipeIngredientFood? Food { get; set; }
    
    [JsonPropertyName("quantity")]
    public double? Quantity { get; set; }
    
    [JsonPropertyName("unit")]
    public RecipeIngredientUnit? Unit { get; set; }
    
    [JsonPropertyName("note")]
    public string? Note { get; set; }
}

public class RecipeIngredientFood
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class RecipeIngredientUnit
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class RecipeInstruction
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    [JsonPropertyName("ingredientReferences")]
    public List<object>? IngredientReferences { get; set; } = [];
}
