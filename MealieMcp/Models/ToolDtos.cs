using MealieMcp.Clients.Models;
using Microsoft.Kiota.Abstractions.Serialization;

namespace MealieMcp.Models;

public record FoodSummaryDto(
    string? Id, 
    string? Name, 
    string? PluralName, 
    string? Description, 
    MultiPurposeLabelSummary? Label
);

public record FoodDetailDto(
    string? Id, 
    string? Name, 
    string? PluralName, 
    string? Description, 
    List<IngredientFoodAlias>? Aliases, 
    MultiPurposeLabelSummary? Label, 
    UntypedNode? Extras
);

public record RecipeSummaryDto(
    string? Id,
    string? Name,
    string? Slug,
    string? Description,
    Microsoft.Kiota.Abstractions.Date? DateAdded,
    DateTimeOffset? DateUpdated,
    object? Rating, // Rating is complex in model
    double? RecipeServings,
    double? RecipeYieldQuantity
);

public record RecipeDetailDto(
    string? Id,
    string? Name,
    string? Slug,
    string? Description,
    Microsoft.Kiota.Abstractions.Date? DateAdded,
    DateTimeOffset? DateUpdated,
    object? Rating,
    double? RecipeServings,
    double? RecipeYieldQuantity,
    object? CookTime,
    object? PrepTime,
    object? TotalTime,
    object? PerformTime,
    List<RecipeCategory>? RecipeCategory,
    List<RecipeTag>? Tags,
    List<RecipeTool>? Tools,
    List<RecipeIngredientOutput>? RecipeIngredient,
    List<RecipeStep>? RecipeInstructions
);

public record TagDto(
    string? Id,
    string? Name,
    string? Slug
);
