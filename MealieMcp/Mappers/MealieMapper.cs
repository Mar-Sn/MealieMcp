using MealieMcp.Clients.Models;
using MealieMcp.Models;
using Riok.Mapperly.Abstractions;

namespace MealieMcp.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class MealieMapper
{
    // Food Mappings
    public static partial FoodSummaryDto ToSummary(this IngredientFoodOutput food);
    public static partial FoodDetailDto ToDetail(this IngredientFoodOutput food);

    [MapProperty(nameof(IngredientFoodOutput.Id), nameof(CreateIngredientFood.Id))]
    [MapProperty(nameof(IngredientFoodOutput.LabelId), nameof(CreateIngredientFood.LabelId))]
    [MapProperty(nameof(IngredientFoodOutput.PluralName), nameof(CreateIngredientFood.PluralName))]
    public static partial CreateIngredientFood ToCreateIngredientFood(this IngredientFoodOutput food);
    
    // Helpers for Food
    private static string? MapPluralNameToString(IngredientFoodOutput.IngredientFoodOutput_pluralName? source) => 
        source?.String ?? source?.IngredientFoodOutputPluralNameMember1?.ToString();

    private static CreateIngredientFood.CreateIngredientFood_id? MapIdToCreateId(string? id) =>
        id == null ? null : new CreateIngredientFood.CreateIngredientFood_id { String = id };

    private static CreateIngredientFood.CreateIngredientFood_labelId? MapLabelIdToCreateLabelId(IngredientFoodOutput.IngredientFoodOutput_labelId? source) =>
        source == null ? null : new CreateIngredientFood.CreateIngredientFood_labelId { String = source.String ?? source.IngredientFoodOutputLabelIdMember1?.ToString() };

    private static CreateIngredientFood.CreateIngredientFood_pluralName? MapPluralNameToCreatePluralName(IngredientFoodOutput.IngredientFoodOutput_pluralName? source) =>
        source == null ? null : new CreateIngredientFood.CreateIngredientFood_pluralName { String = source.String ?? source.IngredientFoodOutputPluralNameMember1?.ToString() };

    private static CreateIngredientFoodAlias MapAlias(IngredientFoodAlias alias) => 
        new CreateIngredientFoodAlias { Name = alias.Name, AdditionalData = alias.AdditionalData };


    // Recipe Mappings
    public static partial RecipeSummaryDto ToSummary(this RecipeSummary recipe);
    public static partial RecipeDetailDto ToDetail(this RecipeOutput recipe);

    // Helpers for Recipe Summary
    private static Microsoft.Kiota.Abstractions.Date? MapDateAddedSummary(RecipeSummary.RecipeSummary_dateAdded? dateAdded) =>
        dateAdded?.DateOnly;

    private static DateTimeOffset? MapDateUpdatedSummary(RecipeSummary.RecipeSummary_dateUpdated? dateUpdated) =>
        dateUpdated?.DateTimeOffset;

    // Helpers for Recipe Output
    private static string? MapRecipeOutputName(RecipeOutput.RecipeOutput_name? name) =>
        name?.String ?? name?.RecipeOutputNameMember1?.ToString();

    private static Microsoft.Kiota.Abstractions.Date? MapDateAddedOutput(RecipeOutput.RecipeOutput_dateAdded? dateAdded) =>
        dateAdded?.DateOnly;

    private static DateTimeOffset? MapDateUpdatedOutput(RecipeOutput.RecipeOutput_dateUpdated? dateUpdated) =>
        dateUpdated?.DateTimeOffset;

    private static object? MapRecipeOutputRating(RecipeOutput.RecipeOutput_rating? rating) =>
        rating?.Double != null ? rating.Double : rating?.RecipeOutputRatingMember1;

    private static object? MapRecipeOutputCookTime(RecipeOutput.RecipeOutput_cookTime? time) =>
        time?.String ?? (object?)time?.RecipeOutputCookTimeMember1;

    private static object? MapRecipeOutputPrepTime(RecipeOutput.RecipeOutput_prepTime? time) =>
        time?.String ?? (object?)time?.RecipeOutputPrepTimeMember1;

    private static object? MapRecipeOutputTotalTime(RecipeOutput.RecipeOutput_totalTime? time) =>
        time?.String ?? (object?)time?.RecipeOutputTotalTimeMember1;

    private static object? MapRecipeOutputPerformTime(RecipeOutput.RecipeOutput_performTime? time) =>
        time?.String ?? (object?)time?.RecipeOutputPerformTimeMember1;
        
    // Tag Mappings
    public static partial TagDto ToDto(this RecipeTagResponse tag);
    public static partial TagDto ToDto(this RecipeTag tag);

    private static string? MapRecipeTagId(RecipeTag.RecipeTag_id? id) =>
        id?.String ?? id?.RecipeTagIdMember1?.ToString();
}
