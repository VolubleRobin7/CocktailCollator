using CocktailCollator.Application.Models;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeInputPort
{
    public List<CreateRecipeInputPortRecipeIngredient> Ingredients { get; set; } = [];
    public required string Name { get; set; }
    public List<CreateRecipeInputPortStep> Steps { get; set; } = [];
    public DocumentModel? Images { get; set; }
}

public class CreateRecipeInputPortRecipeIngredient
{
    public decimal Amount { get; set; } = 1m;
    public CreateRecipeInputPortIngredient? Ingredient { get; set; }
    public Guid IngredientId { get; set; }
    public required Guid MeasurementId { get; set; }
}

public class CreateRecipeInputPortIngredient
{
    public required string Name { get; set; }
}

public class CreateRecipeInputPortStep
{
    public required string Instruction { get; set; }
    public required int Order { get; set; }
}
