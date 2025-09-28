using CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels;

public class RecipeViewModel
{
    public IAsyncRelayCommand Command { get; set; }

    public RecipeViewModel(CreateRecipeInteractor createRecipeInteractor)
    {
        this.Command = new AsyncRelayCommand(cancellationToken
            => createRecipeInteractor.Interact(
                new() { Name = "TestRecipe", Ingredients = [new() { Name = "TestIngredient" }] },
                new CreateRecipePresenter(),
                default));
    }

    private class CreateRecipePresenter : ICreateRecipeOutputPort
    {
        Task ICreateRecipeOutputPort.Success(Recipe recipe, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private class CreateIngredientPresenter : ICreateIngredientOutputPort
    {
        Task ICreateIngredientOutputPort.Success(Ingredient ingredient, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
