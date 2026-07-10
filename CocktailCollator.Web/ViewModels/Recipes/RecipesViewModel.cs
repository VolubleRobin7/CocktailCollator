using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;
using CocktailCollator.Application.UseCases.Recipes.GetRecipes;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;
using CocktailCollator.Domain.Entities;
using CocktailCollator.Web.Common;
using CocktailCollator.Web.Views.Components.Toasts;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipesViewModel
{
    public IAsyncRelayCommand<CreateRecipeInputPort> CreateCommand { get; set; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; set; }
    public IAsyncRelayCommand GetCommand { get; set; }
    public IAsyncRelayCommand<UpdateRecipeInputPort> UpdateCommand { get; set; }

    public List<RecipeViewModel> Recipes { get; private set; } = [];


    public RecipesViewModel(
        CreateRecipeInteractor createRecipeInteractor,
        DeleteRecipeInteractor deleteRecipeInteractor,
        GetRecipesInteractor getRecipesInteractor,
        UpdateRecipeInteractor updateRecipeInteractor,
        IMapper mapper,
        ToastService toastService)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateRecipeInputPort>((inputPort, cancellationToken)
            => createRecipeInteractor.Interact(
                inputPort,
                new CreateRecipePresenter(mapper, toastService, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((recipeId, cancellationToken)
            => deleteRecipeInteractor.Interact(
                new() { RecipeId = recipeId },
                new DeleteRecipePresenter(toastService, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getRecipesInteractor.Interact(
                new GetRecipesPresenter(mapper, this),
                cancellationToken));

        this.UpdateCommand = new AsyncRelayCommand<UpdateRecipeInputPort>((inputPort, cancellationToken)
            => updateRecipeInteractor.InteractAsync(
                inputPort,
                new UpdateRecipePresenter(mapper, toastService, this),
                cancellationToken));
    }

    private class CreateRecipePresenter(IMapper mapper, ToastService toastService, RecipesViewModel viewModel) : ICreateRecipeOutputPort
    {
        Task ICreateRecipeOutputPort.Success(Recipe recipe, CancellationToken cancellationToken)
        {
            viewModel.Recipes.Add(mapper.Map<RecipeViewModel>(recipe));
            toastService.ShowToast(ToastType.Success, "Recipe Created", $"{recipe.Name} created successfully");
            return Task.CompletedTask;
        }
    }

    private class DeleteRecipePresenter(ToastService toastService, RecipesViewModel viewModel) : IDeleteRecipeOutputPort
    {
        Task IDeleteRecipeOutputPort.Success(Recipe deletedRecipe, CancellationToken cancellationToken)
        {
            _ = viewModel.Recipes.RemoveAll(recipe => recipe.RecipeId == deletedRecipe.RecipeId);
            toastService.ShowToast(ToastType.Info, "Recipe Deleted", $"{deletedRecipe.Name} deleted successfully");
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

    private class UpdateRecipePresenter(IMapper mapper, ToastService toastService, RecipesViewModel viewModel) : IUpdateRecipeOutputPort
    {
        Task IUpdateRecipeOutputPort.Failure(string failureReason, Recipe? recipe, CancellationToken cancellationToken)
        {
            toastService.ShowToast(ToastType.Danger, "Failed to Update", failureReason);
            return Task.CompletedTask;
        }

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

            toastService.ShowToast(ToastType.Success, "Recipe Updated", $"{recipe.Name} updated successfully");
            return Task.CompletedTask;
        }
    }
}
