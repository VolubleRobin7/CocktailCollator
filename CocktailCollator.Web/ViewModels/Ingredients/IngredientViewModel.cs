using CocktailCollator.Web.ViewModels.Recipes;

namespace CocktailCollator.Web.ViewModels.Ingredients;

public class IngredientViewModel
{
    public required Guid IngredientId { get; set; }
    public string? Name { get; set; }
    public List<RecipeViewModel>? Recipes { get; set; }
}
