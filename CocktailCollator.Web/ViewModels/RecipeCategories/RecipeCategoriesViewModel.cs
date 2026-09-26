using AutoMapper;
using CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;
using CocktailCollator.Web.Common.Presenters;
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
        IPipeline<CreateRecipeCategoryInputPort, ICreateRecipeCategoryOutputPort> createRecipeCategoryPipeline,
        IPipeline<DeleteRecipeCategoryInputPort, IDeleteRecipeCategoryOutputPort> deleteRecipeCategoryPipeline,
        IPipeline<IGetRecipeCategoriesOutputPort> getRecipeCategoriesPipeline,
        IMapper mapper,
        IViewModelStore store,
        ToastService toastService)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateRecipeCategoryInputPort>((inputPort, cancellationToken)
            => createRecipeCategoryPipeline.ExecuteAsync(
                inputPort,
                new CreateRecipeCategoryPresenter(mapper, store, toastService, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((categoryId, cancellationToken)
            => deleteRecipeCategoryPipeline.ExecuteAsync(
                new() { RecipeCategoryId = categoryId },
                new DeleteRecipeCategoryPresenter(store, toastService, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getRecipeCategoriesPipeline.ExecuteAsync(
                new GetRecipeCategoriesPresenter(mapper, store, toastService, this),
                cancellationToken));
    }

    private class CreateRecipeCategoryPresenter(IMapper mapper, IViewModelStore store, ToastService toastService, RecipeCategoriesViewModel viewModel)
        : BasePresenter(toastService, "create recipe categories"), ICreateRecipeCategoryOutputPort
    {
        Task ICreateRecipeCategoryOutputPort.Success(RecipeCategory recipeCategory, CancellationToken cancellationToken)
        {
            var _Category = mapper.Map<RecipeCategoryViewModel>(recipeCategory);
            viewModel.RecipeCategories.Add(store.UpdateOrRegister(_Category.RecipeCategoryId, _Category));
            this.ToastService.ShowToast(ToastType.Success, "Category Created", $"{recipeCategory.Name} created successfully");
            return Task.CompletedTask;
        }
    }

    private class DeleteRecipeCategoryPresenter(IViewModelStore store, ToastService toastService, RecipeCategoriesViewModel viewModel)
        : BasePresenter(toastService, "delete recipe categories"), IDeleteRecipeCategoryOutputPort
    {
        Task IDeleteRecipeCategoryOutputPort.StillInUse(string reason, RecipeCategory? category, CancellationToken cancellationToken)
        {
            this.ToastService.ShowToast(ToastType.Danger, "Failed to Delete", reason);
            return Task.CompletedTask;
        }

        Task IDeleteRecipeCategoryOutputPort.Success(RecipeCategory deletedCategory, CancellationToken cancellationToken)
        {
            _ = viewModel.RecipeCategories.RemoveAll(c => c.RecipeCategoryId == deletedCategory.RecipeCategoryId);
            store.Remove<RecipeCategoryViewModel>(deletedCategory.RecipeCategoryId);
            this.ToastService.ShowToast(ToastType.Info, "Category Deleted", $"{deletedCategory.Name} deleted successfully");
            return Task.CompletedTask;
        }
    }

    private class GetRecipeCategoriesPresenter(IMapper mapper, IViewModelStore store, ToastService toastService, RecipeCategoriesViewModel viewModel)
        : BasePresenter(toastService, "view recipe categories"), IGetRecipeCategoriesOutputPort
    {
        Task IGetRecipeCategoriesOutputPort.Success(List<RecipeCategory> recipeCategories, CancellationToken cancellationToken)
        {
            viewModel.RecipeCategories = [.. mapper.Map<List<RecipeCategoryViewModel>>(recipeCategories).Select(c => store.UpdateOrRegister(c.RecipeCategoryId, c))];
            return Task.CompletedTask;
        }
    }
}
