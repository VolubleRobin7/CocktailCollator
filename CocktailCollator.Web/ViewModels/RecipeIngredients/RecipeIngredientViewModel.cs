using CocktailCollator.Web.ViewModels.Ingredients;
using CocktailCollator.Web.ViewModels.Measurements;
using CocktailCollator.Web.ViewModels.Recipes;

namespace CocktailCollator.Web.ViewModels.RecipeIngredients;

public class RecipeIngredientViewModel
{
    public decimal? Amount { get; set; }
    public IngredientViewModel? Ingredient { get; set; }
    public required Guid IngredientId { get; set; }
    public MeasurementViewModel? Measurement { get; set; }
    public required Guid MeasurementId { get; set; }
    public RecipeViewModel? Recipe { get; set; }
    public required Guid RecipeId { get; set; }
}
