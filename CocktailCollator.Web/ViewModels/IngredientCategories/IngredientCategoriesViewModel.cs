using AutoMapper;
using CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;
using CocktailCollator.Application.UseCases.IngredientCategories.DeleteIngredientCategory;
using CocktailCollator.Application.UseCases.IngredientCategories.GetIngredientCategories;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.IngredientCategories;

public class IngredientCategoriesViewModel
{
    public IAsyncRelayCommand<CreateIngredientCategoryInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }

    public List<IngredientCategoryViewModel> IngredientCategories { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public IngredientCategoriesViewModel(
        CreateIngredientCategoryInteractor createIngredientCategoryInteractor,
        DeleteIngredientCategoryInteractor deleteIngredientCategoryInteractor,
        GetIngredientCategoriesInteractor getIngredientCategoriesInteractor,
        IMapper mapper)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateIngredientCategoryInputPort>((inputPort, cancellationToken)
            => createIngredientCategoryInteractor.Interact(
                inputPort,
                new CreateIngredientCategoryPresenter(mapper, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((categoryId, cancellationToken)
            => deleteIngredientCategoryInteractor.Interact(
                new() { IngredientCategoryId = categoryId },
                new DeleteIngredientCategoryPresenter(this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getIngredientCategoriesInteractor.Interact(
                new GetIngredientCategoriesPresenter(mapper, this),
                cancellationToken));
    }

    private class CreateIngredientCategoryPresenter(IMapper mapper, IngredientCategoriesViewModel viewModel) : ICreateIngredientCategoryOutputPort
    {
        Task ICreateIngredientCategoryOutputPort.Success(IngredientCategory ingredientCategory, CancellationToken cancellationToken)
        {
            viewModel.IngredientCategories.Add(mapper.Map<IngredientCategoryViewModel>(ingredientCategory));
            return Task.CompletedTask;
        }
    }

    private class DeleteIngredientCategoryPresenter(IngredientCategoriesViewModel viewModel) : IDeleteIngredientCategoryOutputPort
    {
        Task IDeleteIngredientCategoryOutputPort.Failure(string reason, IngredientCategory? category, CancellationToken cancellationToken)
        {
            viewModel.Error = reason;
            return Task.CompletedTask;
        }

        Task IDeleteIngredientCategoryOutputPort.Success(IngredientCategory deletedCategory, CancellationToken cancellationToken)
        {
            _ = viewModel.IngredientCategories.RemoveAll(c => c.IngredientCategoryId == deletedCategory.IngredientCategoryId);
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
