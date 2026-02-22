using CocktailCollator.Web.ViewModels.IngredientCategories;
using CocktailCollator.Web.ViewModels.Measurements;
using CocktailCollator.Web.ViewModels.RecipeIngredients;

namespace CocktailCollator.Web.ViewModels.Ingredients;

public class IngredientViewModel
{
    public IngredientCategoryViewModel? Category { get; set; }
    public required Guid IngredientId { get; set; }
    public List<MeasurementViewModel>? Measurements { get; set; }
    public string? Name { get; set; }
    public List<RecipeIngredientViewModel>? Recipes { get; set; }
}
