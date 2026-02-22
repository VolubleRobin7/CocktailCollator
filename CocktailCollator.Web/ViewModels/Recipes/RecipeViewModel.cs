using CocktailCollator.Web.ViewModels.RecipeIngredients;
using CocktailCollator.Web.ViewModels.RecipeSteps;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipeViewModel
{
    public List<RecipeIngredientViewModel>? Ingredients { get; set; }
    public string? Name { get; set; }
    public required Guid RecipeId { get; set; }
    public List<RecipeStepViewModel>? Steps { get; set; }
}