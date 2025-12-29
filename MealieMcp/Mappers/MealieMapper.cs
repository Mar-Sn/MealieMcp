using MealieMcp.Client.Models.Foods;
using MealieMcp.Client.Models.Recipes;
using MealieMcp.Client.Models.Tags;
using MealieMcp.Models;
using Riok.Mapperly.Abstractions;

namespace MealieMcp.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class MealieMapper
{
    // Food Mappings
    [MapProperty(nameof(Food.Label.Name), nameof(FoodSummaryDto.Label))]
    public static partial FoodSummaryDto ToSummary(this Food food);

    [MapProperty(nameof(Food.Label.Name), nameof(FoodDetailDto.Label))]
    public static partial FoodDetailDto ToDetail(this Food food);

    public static partial CreateIngredientFood ToCreateIngredientFood(this Food food);

    private static string? MapFoodAlias(FoodAlias alias) => alias.Alias;

    // Recipe Mappings
    public static partial RecipeSummaryDto ToSummary(this RecipeSummary recipe);
    public static partial RecipeDetailDto ToDetail(this RecipeDetail recipe);

    public static partial RecipeInput ToRecipeInput(this RecipeDetail recipe);

    // Tag Mappings
    public static partial TagDto ToDto(this Tag tag);

    // Recipe Details Sub-Object Mappings
    public static partial RecipeCategoryDto ToRecipeCategoryDto(this RecipeCategory category);
    public static partial RecipeTagDto ToRecipeTagDto(this RecipeTag tag);
    public static partial RecipeToolDto ToDto(this RecipeTool tool);
    
    [MapProperty(nameof(RecipeIngredient.Food.Name), nameof(RecipeIngredientDto.Food))]
    [MapProperty(nameof(RecipeIngredient.Unit.Name), nameof(RecipeIngredientDto.Unit))]
    public static partial RecipeIngredientDto ToDto(this RecipeIngredient ingredient);

    public static partial RecipeInstructionDto ToDto(this RecipeInstruction instruction);
}