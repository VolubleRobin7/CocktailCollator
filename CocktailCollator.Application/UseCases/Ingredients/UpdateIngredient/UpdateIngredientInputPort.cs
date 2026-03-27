namespace CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;

public class UpdateIngredientInputPort
{
    public required Guid IngredientId { get; set; }
    public required string Name { get; set; }
    public Guid? IngredientCategoryId { get; set; }
}
