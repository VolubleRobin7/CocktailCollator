using CocktailCollator.Web.Common.State;
using CocktailCollator.Web.ViewModels.Ingredients;
using CocktailCollator.Web.ViewModels.Measurements;
using CocktailCollator.Web.ViewModels.Recipes;

namespace CocktailCollator.Web.ViewModels.RecipeIngredients;

public class RecipeIngredientViewModel : IStoreableViewModel<RecipeIngredientViewModel>
{
    public decimal? Amount { get; set; }
    public IngredientViewModel? Ingredient { get; set; }
    public required Guid IngredientId { get; set; }
    public MeasurementViewModel? Measurement { get; set; }
    public required Guid MeasurementId { get; set; }
    public RecipeViewModel? Recipe { get; set; }
    public required Guid RecipeId { get; set; }

    public void ApplyChanges(RecipeIngredientViewModel source, IViewModelStore store)
    {
        this.Amount = source.Amount;
        this.IngredientId = source.IngredientId;
        this.MeasurementId = source.MeasurementId;
        this.RecipeId = source.RecipeId;

        this.Ingredient = source.Ingredient is not null
            ? store.UpdateOrRegister(source.IngredientId, source.Ingredient)
            : store.Get<IngredientViewModel>(source.IngredientId);

        this.Measurement = source.Measurement is not null
            ? store.UpdateOrRegister(source.MeasurementId, source.Measurement)
            : store.Get<MeasurementViewModel>(source.MeasurementId);

        // No update here to prevent a stack overflow when called from RecipeViewModel
        this.Recipe = source.Recipe is not null
            ? store.GetOrRegister(source.RecipeId, source.Recipe)
            : store.Get<RecipeViewModel>(source.RecipeId);
    }
}
