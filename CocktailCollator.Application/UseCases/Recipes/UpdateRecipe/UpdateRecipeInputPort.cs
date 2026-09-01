using CocktailCollator.Application.Models;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeInputPort
{
    public List<DocumentModel> Images { get; set; } = [];
    public List<UpdateRecipeInputPortRecipeIngredient> Ingredients { get; set; } = [];
    public required string Name { get; set; }
    public required Guid RecipeId { get; set; }
    public Guid? RecipeCategoryId { get; set; }
    public List<UpdateRecipeInputPortStep> Steps { get; set; } = [];
    public string? GlobalNote { get; set; }
}

public class UpdateRecipeInputPortRecipeIngredient
{
    public decimal Amount { get; set; } = 1m;
    public UpdateRecipeInputPortIngredient? Ingredient { get; set; }
    public Guid IngredientId { get; set; }
    public required Guid MeasurementId { get; set; }
}

public class UpdateRecipeInputPortIngredient
{
    public required string Name { get; set; }
}

public class UpdateRecipeInputPortStep
{
    public required string Instruction { get; set; }
    public required int Order { get; set; }
}
