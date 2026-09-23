using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;
using CocktailCollator.Application.UseCases.Recipes.GetRecipes;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;
using CocktailCollator.Web.Common.Presenters;
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
        IPipeline<CreateRecipeInputPort, ICreateRecipeOutputPort> createRecipePipeline,
        IPipeline<DeleteRecipeInputPort, IDeleteRecipeOutputPort> deleteRecipePipeline,
        IPipeline<GetRecipesInputPort, IGetRecipesOutputPort> getRecipesPipeline,
        IPipeline<UpdateRecipeInputPort, IUpdateRecipeOutputPort> updateRecipePipeline,
        IMapper mapper,
        IViewModelStore store,
        ToastService toastService)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateRecipeInputPort>((inputPort, cancellationToken)
            => createRecipePipeline.ExecuteAsync(
                inputPort,
                new CreateRecipePresenter(mapper, store, toastService, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((recipeId, cancellationToken)
            => deleteRecipePipeline.ExecuteAsync(
                new() { RecipeId = recipeId },
                new DeleteRecipePresenter(store, toastService, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getRecipesPipeline.ExecuteAsync(
                new GetRecipesInputPort(),
                new GetRecipesPresenter(mapper, store, toastService, this),
                cancellationToken));

        this.UpdateCommand = new AsyncRelayCommand<UpdateRecipeInputPort>((inputPort, cancellationToken)
            => updateRecipePipeline.ExecuteAsync(
                inputPort,
                new UpdateRecipePresenter(mapper, store, toastService),
                cancellationToken));
    }

    private class CreateRecipePresenter(IMapper mapper, IViewModelStore store, ToastService toastService, RecipesViewModel viewModel)
        : BasePresenter(toastService, "create recipes"), ICreateRecipeOutputPort
    {
        Task ICreateRecipeOutputPort.Success(Recipe recipe, CancellationToken cancellationToken)
        {
            var _Recipe = mapper.Map<RecipeViewModel>(recipe);
            viewModel.Recipes.Add(store.UpdateOrRegister(_Recipe.RecipeId, _Recipe));
            toastService.ShowToast(ToastType.Success, "Recipe Created", $"{recipe.Name} created successfully");
            return Task.CompletedTask;
        }
    }

    private class DeleteRecipePresenter(IViewModelStore store, ToastService toastService, RecipesViewModel viewModel)
        : BasePresenter(toastService, "delete recipes"), IDeleteRecipeOutputPort
    {
        Task IDeleteRecipeOutputPort.Success(Recipe deletedRecipe, CancellationToken cancellationToken)
        {
            _ = viewModel.Recipes.RemoveAll(recipe => recipe.RecipeId == deletedRecipe.RecipeId);
            store.Remove<RecipeViewModel>(deletedRecipe.RecipeId);
            toastService.ShowToast(ToastType.Info, "Recipe Deleted", $"{deletedRecipe.Name} deleted successfully");
            return Task.CompletedTask;
        }
    }

    private class GetRecipesPresenter(IMapper mapper, IViewModelStore store, ToastService toastService, RecipesViewModel viewModel)
        : BasePresenter(toastService, "view recipes"), IGetRecipesOutputPort
    {
        Task IGetRecipesOutputPort.Success(List<Recipe> recipes, CancellationToken cancellationToken)
        {
            viewModel.Recipes = [.. mapper.Map<List<RecipeViewModel>>(recipes).Select(r => store.UpdateOrRegister(r.RecipeId, r))];
            return Task.CompletedTask;
        }
    }

    private class UpdateRecipePresenter(IMapper mapper, IViewModelStore store, ToastService toastService)
        : BasePresenter(toastService, "update recipes"), IUpdateRecipeOutputPort
    {
        public override Task NotFound(CancellationToken cancellationToken)
        {
            toastService.ShowToast(ToastType.Warning, "Recipe Not Found", "The recipe you are trying to update does not exist");
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
