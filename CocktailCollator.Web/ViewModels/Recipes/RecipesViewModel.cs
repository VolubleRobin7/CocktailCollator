using AutoMapper;
using CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;
using CocktailCollator.Application.UseCases.Recipes.GetRecipes;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipesViewModel
{
    public IAsyncRelayCommand CreateCommand { get; set; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; set; }
    public IAsyncRelayCommand GetCommand { get; set; }

    public List<RecipeViewModel> Recipes { get; private set; } = [];

    public RecipesViewModel(
        CreateRecipeInteractor createRecipeInteractor,
        DeleteRecipeInteractor deleteRecipeInteractor,
        GetRecipesInteractor getRecipesInteractor,
        IMapper mapper)
    {
        this.CreateCommand = new AsyncRelayCommand(cancellationToken
            => createRecipeInteractor.Interact(
                new() { Name = "TestRecipe", Ingredients = [new() { Name = "TestIngredient" }] },
                new CreateRecipePresenter(mapper, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((recipeId, cancellationToken)
            => deleteRecipeInteractor.Interact(
                new() { RecipeId = recipeId },
                new DeleteRecipePresenter(this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getRecipesInteractor.Interact(
                new GetRecipesPresenter(mapper, this),
                cancellationToken));
    }

    private class CreateRecipePresenter(IMapper mapper, RecipesViewModel viewModel) : ICreateRecipeOutputPort
    {
        Task ICreateRecipeOutputPort.Success(Recipe recipe, CancellationToken cancellationToken)
        {
            viewModel.Recipes.Add(mapper.Map<RecipeViewModel>(recipe));
            return Task.CompletedTask;
        }
    }

    private class DeleteRecipePresenter(RecipesViewModel viewModel) : IDeleteRecipeOutputPort
    {
        Task IDeleteRecipeOutputPort.Success(Recipe deletedRecipe, CancellationToken cancellationToken)
        {
            viewModel.Recipes.RemoveAll(recipe => recipe.RecipeId == deletedRecipe.RecipeId);
            return Task.CompletedTask;
        }
    }

    private class GetRecipesPresenter(IMapper mapper, RecipesViewModel viewModel) : IGetRecipesOutputPort
    {
        Task IGetRecipesOutputPort.Success(List<Recipe> recipes, CancellationToken cancellationToken)
        {
            viewModel.Recipes = mapper.Map<List<RecipeViewModel>>(recipes);
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
