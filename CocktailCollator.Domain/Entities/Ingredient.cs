namespace CocktailCollator.Domain.Entities;

public class Ingredient
{
    public Guid IngredientId { get; set; }
    public required string Name { get; set; }
    public Guid? IngredientCategoryId { get; set; }

    public ICollection<RecipeIngredient>? Recipes { get; set; }
    public ICollection<IngredientMeasurement>? Measurements { get; set; }
    public IngredientCategory? Category { get; set; }
}
