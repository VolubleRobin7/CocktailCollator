namespace CocktailCollator.Domain.Entities;

public class Ingredient
{
    public Guid IngredientId { get; set; }
    public required string Name { get; set; }

    public ICollection<RecipeIngredient>? Recipes { get; set; }
}
