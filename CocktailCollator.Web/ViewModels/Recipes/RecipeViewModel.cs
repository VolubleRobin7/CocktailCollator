using CocktailCollator.Web.ViewModels.Ingredients;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipeViewModel
{
    public List<IngredientViewModel>? Ingredients { get; set; }
    public string? Name { get; set; }
    public required Guid RecipeId { get; set; }
}