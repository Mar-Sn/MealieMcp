namespace MealieMcp.Models;

public record FoodSummaryDto(
    string? Id,
    string? Name,
    string? PluralName,
    string? Description,
    string? Label
);

public record FoodDetailDto(
    string? Id,
    string? Name,
    string? PluralName,
    string? Description,
    List<string?>? Aliases,
    string? Label,
    Dictionary<string, object>? Extras
);

public record RecipeSummaryDto(
    string? Id,
    string? Name,
    string? Slug,
    string? Description,
    DateTime? DateAdded,
    DateTimeOffset? DateUpdated,
    double? Rating,
    double? RecipeServings,
    double? RecipeYieldQuantity
);

public record RecipeDetailDto(
    string? Id,
    string? Name,
    string? Slug,
    string? Description,
    DateTime? DateAdded,
    DateTimeOffset? DateUpdated,
    double? Rating,
    double? RecipeServings,
    double? RecipeYieldQuantity,
    string? CookTime,
    string? PrepTime,
    string? TotalTime,
    string? PerformTime,
    List<RecipeCategoryDto>? RecipeCategory,
    List<RecipeTagDto>? Tags,
    List<RecipeToolDto>? Tools,
    List<RecipeIngredientDto>? RecipeIngredient,
    List<RecipeInstructionDto>? RecipeInstructions
);

public record TagDto(
    string? Id,
    string? Name,
    string? Slug
);

public record RecipeCategoryDto(string? Id, string? Name, string? Slug);
public record RecipeTagDto(string? Id, string? Name, string? Slug);
public record RecipeToolDto(string? Id, string? Name, string? Slug);

public record RecipeIngredientDto(
    string? Display,
    string? Food,
    double? Quantity,
    string? Unit,
    string? Note
);

public record RecipeInstructionDto(
    string? Id,
    string? Title,
    string? Text
);