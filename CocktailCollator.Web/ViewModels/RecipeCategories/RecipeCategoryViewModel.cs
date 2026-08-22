using CocktailCollator.Web.Common.State;

namespace CocktailCollator.Web.ViewModels.RecipeCategories;

public class RecipeCategoryViewModel : IStoreableViewModel<RecipeCategoryViewModel>
{
    public Guid RecipeCategoryId { get; set; }
    public required string Name { get; set; }

    public void ApplyChanges(RecipeCategoryViewModel source, IViewModelStore store)
    {
        this.Name = source.Name;
    }
}
