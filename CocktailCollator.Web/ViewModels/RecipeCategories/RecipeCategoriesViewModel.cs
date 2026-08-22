using AutoMapper;
using CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;
using CocktailCollator.Domain.Entities;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.Common.State;
using CocktailCollator.Web.Views.Components.Toasts;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.RecipeCategories;

public class RecipeCategoriesViewModel
{
    public IAsyncRelayCommand<CreateRecipeCategoryInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }

    public List<RecipeCategoryViewModel> RecipeCategories { get; private set; } = [];


    public RecipeCategoriesViewModel(
        CreateRecipeCategoryInteractor createRecipeCategoryInteractor,
        DeleteRecipeCategoryInteractor deleteRecipeCategoryInteractor,
        GetRecipeCategoriesInteractor getRecipeCategoriesInteractor,
        IMapper mapper,
        IViewModelStore store,
        ToastService toastService)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateRecipeCategoryInputPort>((inputPort, cancellationToken)
            => createRecipeCategoryInteractor.Interact(
                inputPort,
                new CreateRecipeCategoryPresenter(mapper, toastService, store, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((categoryId, cancellationToken)
            => deleteRecipeCategoryInteractor.Interact(
                new() { RecipeCategoryId = categoryId },
                new DeleteRecipeCategoryPresenter(toastService, store, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getRecipeCategoriesInteractor.Interact(
                new GetRecipeCategoriesPresenter(mapper, store, this),
                cancellationToken));
    }

    private class CreateRecipeCategoryPresenter(IMapper mapper, ToastService toastService, IViewModelStore store, RecipeCategoriesViewModel viewModel) : ICreateRecipeCategoryOutputPort
    {
        Task ICreateRecipeCategoryOutputPort.Success(RecipeCategory recipeCategory, CancellationToken cancellationToken)
        {
            var _Category = mapper.Map<RecipeCategoryViewModel>(recipeCategory);
            viewModel.RecipeCategories.Add(store.UpdateOrRegister(_Category.RecipeCategoryId, _Category));
            toastService.ShowToast(ToastType.Success, "Category Created", $"{recipeCategory.Name} created successfully");
            return Task.CompletedTask;
        }
    }

    private class DeleteRecipeCategoryPresenter(ToastService toastService, IViewModelStore store, RecipeCategoriesViewModel viewModel) : IDeleteRecipeCategoryOutputPort
    {
        Task IDeleteRecipeCategoryOutputPort.Failure(string reason, RecipeCategory? category, CancellationToken cancellationToken)
        {
            toastService.ShowToast(ToastType.Danger, "Failed to Delete", reason);
            return Task.CompletedTask;
        }

        Task IDeleteRecipeCategoryOutputPort.Success(RecipeCategory deletedCategory, CancellationToken cancellationToken)
        {
            _ = viewModel.RecipeCategories.RemoveAll(c => c.RecipeCategoryId == deletedCategory.RecipeCategoryId);
            store.Remove<RecipeCategoryViewModel>(deletedCategory.RecipeCategoryId);
            toastService.ShowToast(ToastType.Info, "Category Deleted", $"{deletedCategory.Name} deleted successfully");
            return Task.CompletedTask;
        }
    }

    private class GetRecipeCategoriesPresenter(IMapper mapper, IViewModelStore store, RecipeCategoriesViewModel viewModel) : IGetRecipeCategoriesOutputPort
    {
        Task IGetRecipeCategoriesOutputPort.Success(List<RecipeCategory> recipeCategories, CancellationToken cancellationToken)
        {
            viewModel.RecipeCategories = [.. mapper.Map<List<RecipeCategoryViewModel>>(recipeCategories).Select(c => store.UpdateOrRegister(c.RecipeCategoryId, c))];
            return Task.CompletedTask;
        }
    }
}
