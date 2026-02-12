namespace CocktailCollator.Domain.Entities;

public class IngredientCategory
{
    public Guid IngredientCategoryId { get; set; }
    public required string Name { get; set; }

    public ICollection<Ingredient>? Ingredients { get; set; }
}
