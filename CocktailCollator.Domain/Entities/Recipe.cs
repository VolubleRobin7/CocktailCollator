namespace CocktailCollator.Domain.Entities;

public class Recipe
{
    public Guid RecipeId { get; set; }
    public required string Name { get; set; }
    public Guid? RecipeCategoryId { get; set; }

    public ICollection<RecipeIngredient>? Ingredients { get; set; }
    public ICollection<RecipeStep>? Steps { get; set; }
    public RecipeCategory? Category { get; set; }
}
