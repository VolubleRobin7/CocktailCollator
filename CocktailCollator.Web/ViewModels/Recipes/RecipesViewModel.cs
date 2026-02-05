using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;
using CocktailCollator.Application.UseCases.Recipes.GetRecipes;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipesViewModel
{
    public IAsyncRelayCommand<CreateRecipeInputPort> CreateCommand { get; set; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; set; }
    public IAsyncRelayCommand GetCommand { get; set; }

    public List<RecipeViewModel> Recipes { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public RecipesViewModel(
        CreateRecipeInteractor createRecipeInteractor,
        DeleteRecipeInteractor deleteRecipeInteractor,
        GetRecipesInteractor getRecipesInteractor,
        IMapper mapper)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateRecipeInputPort>((inputPort, cancellationToken)
            => createRecipeInteractor.Interact(
                inputPort,
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
            _ = viewModel.Recipes.RemoveAll(recipe => recipe.RecipeId == deletedRecipe.RecipeId);
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
}
