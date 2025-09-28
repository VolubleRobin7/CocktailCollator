namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeInputPort
{
    public List<CreateRecipeInputPortIngredient> Ingredients { get; set; } = new();
    public required string Name { get; set; }
}

public class CreateRecipeInputPortIngredient
{
    public required string Name { get; set; }
}
