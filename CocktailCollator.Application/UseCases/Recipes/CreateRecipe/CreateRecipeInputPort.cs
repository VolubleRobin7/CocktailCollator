namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeInputPort
{
    public List<CreateRecipeInputPortIngredient> Ingredients { get; set; } = [];
    public required string Name { get; set; }
    public List<CreateRecipeInputPortStep> Steps { get; set; } = [];
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
