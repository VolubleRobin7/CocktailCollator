using CocktailCollator.Web.Common.State;
using CocktailCollator.Web.ViewModels.IngredientCategories;
using CocktailCollator.Web.ViewModels.Measurements;
using CocktailCollator.Web.ViewModels.RecipeIngredients;

namespace CocktailCollator.Web.ViewModels.Ingredients;

public class IngredientViewModel : IStoreableViewModel<IngredientViewModel>
{
    public IngredientCategoryViewModel? Category { get; set; }
    public required Guid IngredientId { get; set; }
    public List<MeasurementViewModel>? Measurements { get; set; }
    public string? Name { get; set; }
    public List<RecipeIngredientViewModel>? Recipes { get; set; }

    public void ApplyChanges(IngredientViewModel source, IViewModelStore store)
    {
        this.Name = source.Name;

        if (source.Category is not null)
            this.Category = store.UpdateOrRegister(source.Category.IngredientCategoryId, source.Category);

        if (source.Measurements is not null)
            this.Measurements = [.. source.Measurements.Select(m => store.UpdateOrRegister(m.MeasurementId, m))];

        if (source.Recipes is not null)
            this.Recipes = [.. source.Recipes.Select(r => store.UpdateOrRegister(r.RecipeId, r))];
    }
}
