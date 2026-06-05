namespace CocktailCollator.Application.UseCases.RecipeCategories.UpdateRecipeCategory;

public class UpdateRecipeCategoryInputPort
{
    public required Guid RecipeCategoryId { get; set; }
    public required string Name { get; set; }
}
