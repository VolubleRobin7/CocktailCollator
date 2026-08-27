using CocktailCollator.Web.Common.State;

namespace CocktailCollator.Web.ViewModels.IngredientCategories;

public class IngredientCategoryViewModel : IStoreableViewModel<IngredientCategoryViewModel>
{
    public required Guid IngredientCategoryId { get; set; }
    public string? Name { get; set; }

    public void ApplyChanges(IngredientCategoryViewModel source, IViewModelStore store)
    {
        this.Name = source.Name;
    }
}
