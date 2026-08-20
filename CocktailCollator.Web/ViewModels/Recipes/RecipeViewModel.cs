using CocktailCollator.Web.Common.State;
using CocktailCollator.Web.ViewModels.Documents;
using CocktailCollator.Web.ViewModels.RecipeCategories;
using CocktailCollator.Web.ViewModels.RecipeIngredients;
using CocktailCollator.Web.ViewModels.RecipeSteps;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipeViewModel : IStoreableViewModel<RecipeViewModel>
{
    public RecipeCategoryViewModel? Category { get; set; }
    public List<RecipeIngredientViewModel>? Ingredients { get; set; }
    public string? Name { get; set; }
    public required Guid RecipeId { get; set; }
    public List<RecipeStepViewModel>? Steps { get; set; }
    public List<DocumentViewModel>? Images { get; set; }

    public void ApplyChanges(RecipeViewModel source, IViewModelStore store)
    {
        this.Name = source.Name;

        if (source.Category is not null)
            this.Category = store.UpdateOrRegister(source.Category.RecipeCategoryId, source.Category);

        if (source.Ingredients is not null)
            this.Ingredients = [.. source.Ingredients.Select(i => store.UpdateOrRegister(i.IngredientId, i))];

        if (source.Steps is not null)
            this.Steps = [.. source.Steps.Select(s => store.UpdateOrRegister(s.RecipeStepId, s))];

        if (source.Images is not null)
            this.Images = [.. source.Images.Select(i => store.UpdateOrRegister(i.DocumentId, i))];
    }
}