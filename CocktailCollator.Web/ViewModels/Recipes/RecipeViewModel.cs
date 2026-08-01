using CocktailCollator.Web.ViewModels.Documents;
using CocktailCollator.Web.ViewModels.RecipeCategories;
using CocktailCollator.Web.ViewModels.RecipeIngredients;
using CocktailCollator.Web.ViewModels.RecipeSteps;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipeViewModel
{
    public RecipeCategoryViewModel? Category { get; set; }
    public List<RecipeIngredientViewModel>? Ingredients { get; set; }
    public string? Name { get; set; }
    public required Guid RecipeId { get; set; }
    public List<RecipeStepViewModel>? Steps { get; set; }
    public List<DocumentViewModel>? Images { get; set; }
}