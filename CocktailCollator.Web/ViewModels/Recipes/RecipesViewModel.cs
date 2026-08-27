using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;
using CocktailCollator.Application.UseCases.Recipes.GetRecipes;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;
using CocktailCollator.Domain.Entities;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.Common.State;
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
        IViewModelStore store,
        ToastService toastService)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateRecipeInputPort>((inputPort, cancellationToken)
            => createRecipeInteractor.Interact(
                 inputPort,
                new CreateRecipePresenter(mapper, store, toastService, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((recipeId, cancellationToken)
            => deleteRecipeInteractor.Interact(
                new() { RecipeId = recipeId },
                new DeleteRecipePresenter(store, toastService, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getRecipesInteractor.Interact(
                new GetRecipesPresenter(mapper, store, this),
                cancellationToken));

        this.UpdateCommand = new AsyncRelayCommand<UpdateRecipeInputPort>((inputPort, cancellationToken)
            => updateRecipeInteractor.InteractAsync(
                inputPort,
                new UpdateRecipePresenter(mapper, store, toastService),
                cancellationToken));
    }

    private class CreateRecipePresenter(IMapper mapper, IViewModelStore store, ToastService toastService, RecipesViewModel viewModel) : ICreateRecipeOutputPort
    {
        Task ICreateRecipeOutputPort.Success(Recipe recipe, CancellationToken cancellationToken)
        {
            var _Recipe = mapper.Map<RecipeViewModel>(recipe);
            viewModel.Recipes.Add(store.UpdateOrRegister(_Recipe.RecipeId, _Recipe));
            toastService.ShowToast(ToastType.Success, "Recipe Created", $"{recipe.Name} created successfully");
            return Task.CompletedTask;
        }
    }

    private class DeleteRecipePresenter(IViewModelStore store, ToastService toastService, RecipesViewModel viewModel) : IDeleteRecipeOutputPort
    {
        Task IDeleteRecipeOutputPort.Success(Recipe deletedRecipe, CancellationToken cancellationToken)
        {
            _ = viewModel.Recipes.RemoveAll(recipe => recipe.RecipeId == deletedRecipe.RecipeId);
            store.Remove<RecipeViewModel>(deletedRecipe.RecipeId);
            toastService.ShowToast(ToastType.Info, "Recipe Deleted", $"{deletedRecipe.Name} deleted successfully");
            return Task.CompletedTask;
        }
    }

    private class GetRecipesPresenter(IMapper mapper, IViewModelStore store, RecipesViewModel viewModel) : IGetRecipesOutputPort
    {
        Task IGetRecipesOutputPort.Success(List<Recipe> recipes, CancellationToken cancellationToken)
        {
            viewModel.Recipes = [.. mapper.Map<List<RecipeViewModel>>(recipes).Select(r => store.UpdateOrRegister(r.RecipeId, r))];
            return Task.CompletedTask;
        }
    }

    private class UpdateRecipePresenter(IMapper mapper, IViewModelStore store, ToastService toastService) : IUpdateRecipeOutputPort
    {
        Task IUpdateRecipeOutputPort.Failure(string failureReason, Recipe? recipe, CancellationToken cancellationToken)
        {
            toastService.ShowToast(ToastType.Danger, "Failed to Update", failureReason);
            return Task.CompletedTask;
        }

        Task IUpdateRecipeOutputPort.Success(Recipe recipe, CancellationToken cancellationToken)
        {
            var _Recipe = mapper.Map<RecipeViewModel>(recipe);
            _ = store.UpdateOrRegister(_Recipe.RecipeId, _Recipe);
            toastService.ShowToast(ToastType.Success, "Recipe Updated", $"{recipe.Name} updated successfully");
            return Task.CompletedTask;
        }
    }
}
