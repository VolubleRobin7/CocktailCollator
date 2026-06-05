namespace CocktailCollator.Domain.Entities;

public class RecipeCategory
{
    public Guid RecipeCategoryId { get; set; }
    public required string Name { get; set; }

    public ICollection<Recipe>? Recipes { get; set; }
}
