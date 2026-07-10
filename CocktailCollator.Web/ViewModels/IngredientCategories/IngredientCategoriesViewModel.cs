using AutoMapper;
using CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;
using CocktailCollator.Application.UseCases.IngredientCategories.DeleteIngredientCategory;
using CocktailCollator.Application.UseCases.IngredientCategories.GetIngredientCategories;
using CocktailCollator.Domain.Entities;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.Views.Components.Toasts;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.IngredientCategories;

public class IngredientCategoriesViewModel
{
    public IAsyncRelayCommand<CreateIngredientCategoryInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }

    public List<IngredientCategoryViewModel> IngredientCategories { get; private set; } = [];


    public IngredientCategoriesViewModel(
        CreateIngredientCategoryInteractor createIngredientCategoryInteractor,
        DeleteIngredientCategoryInteractor deleteIngredientCategoryInteractor,
        GetIngredientCategoriesInteractor getIngredientCategoriesInteractor,
        IMapper mapper,
        ToastService toastService)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateIngredientCategoryInputPort>((inputPort, cancellationToken)
            => createIngredientCategoryInteractor.Interact(
                inputPort,
                new CreateIngredientCategoryPresenter(mapper, toastService, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((categoryId, cancellationToken)
            => deleteIngredientCategoryInteractor.Interact(
                new() { IngredientCategoryId = categoryId },
                new DeleteIngredientCategoryPresenter(toastService, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getIngredientCategoriesInteractor.Interact(
                new GetIngredientCategoriesPresenter(mapper, this),
                cancellationToken));
    }

    private class CreateIngredientCategoryPresenter(IMapper mapper, ToastService toastService, IngredientCategoriesViewModel viewModel) : ICreateIngredientCategoryOutputPort
    {
        Task ICreateIngredientCategoryOutputPort.Success(IngredientCategory ingredientCategory, CancellationToken cancellationToken)
        {
            viewModel.IngredientCategories.Add(mapper.Map<IngredientCategoryViewModel>(ingredientCategory));
            toastService.ShowToast(ToastType.Success, "Category Created", $"{ingredientCategory.Name} created successfully");
            return Task.CompletedTask;
        }
    }

    private class DeleteIngredientCategoryPresenter(ToastService toastService, IngredientCategoriesViewModel viewModel) : IDeleteIngredientCategoryOutputPort
    {
        Task IDeleteIngredientCategoryOutputPort.Failure(string reason, IngredientCategory? category, CancellationToken cancellationToken)
        {
            toastService.ShowToast(ToastType.Danger, "Failed to Delete", reason);
            return Task.CompletedTask;
        }

        Task IDeleteIngredientCategoryOutputPort.Success(IngredientCategory deletedCategory, CancellationToken cancellationToken)
        {
            _ = viewModel.IngredientCategories.RemoveAll(c => c.IngredientCategoryId == deletedCategory.IngredientCategoryId);
            toastService.ShowToast(ToastType.Info, "Category Deleted", $"{deletedCategory.Name} deleted successfully");
            return Task.CompletedTask;
        }
    }

    private class GetIngredientCategoriesPresenter(IMapper mapper, IngredientCategoriesViewModel viewModel) : IGetIngredientCategoriesOutputPort
    {
        Task IGetIngredientCategoriesOutputPort.Success(List<IngredientCategory> ingredientCategories, CancellationToken cancellationToken)
        {
            viewModel.IngredientCategories = mapper.Map<List<IngredientCategoryViewModel>>(ingredientCategories);
            return Task.CompletedTask;
        }
    }
}
