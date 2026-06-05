using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;
using CocktailCollator.Application.UseCases.Recipes.GetRecipes;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipesViewModel
{
    public IAsyncRelayCommand<CreateRecipeInputPort> CreateCommand { get; set; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; set; }
    public IAsyncRelayCommand GetCommand { get; set; }
    public IAsyncRelayCommand<UpdateRecipeInputPort> UpdateCommand { get; set; }

    public List<RecipeViewModel> Recipes { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public RecipesViewModel(
        CreateRecipeInteractor createRecipeInteractor,
        DeleteRecipeInteractor deleteRecipeInteractor,
        GetRecipesInteractor getRecipesInteractor,
        UpdateRecipeInteractor updateRecipeInteractor,
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

        this.UpdateCommand = new AsyncRelayCommand<UpdateRecipeInputPort>((inputPort, cancellationToken)
            => updateRecipeInteractor.InteractAsync(
                inputPort,
                new UpdateRecipePresenter(mapper, this),
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

    private class UpdateRecipePresenter(IMapper mapper, RecipesViewModel viewModel) : IUpdateRecipeOutputPort
    {
        Task IUpdateRecipeOutputPort.Failure(string failureReason, Recipe? recipe, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        Task IUpdateRecipeOutputPort.Success(Recipe recipe, CancellationToken cancellationToken)
        {
            var _Existing = viewModel.Recipes.FirstOrDefault(r => r.RecipeId == recipe.RecipeId);
            if (_Existing is not null)
            {
                var _Updated = mapper.Map<RecipeViewModel>(recipe);
                _Existing.Name = _Updated.Name;
                _Existing.Ingredients = _Updated.Ingredients;
                _Existing.Steps = _Updated.Steps;
                _Existing.Category = _Updated.Category;
            }

            return Task.CompletedTask;
        }
    }
}
