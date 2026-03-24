using AutoMapper;
using CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.IngredientCategories;

public class IngredientCategoriesViewModel
{
    public IAsyncRelayCommand<CreateIngredientCategoryInputPort> CreateCommand { get; }

    public List<IngredientCategoryViewModel> IngredientCategories { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public IngredientCategoriesViewModel(
        CreateIngredientCategoryInteractor createIngredientCategoryInteractor,
        IMapper mapper)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateIngredientCategoryInputPort>((inputPort, cancellationToken)
            => createIngredientCategoryInteractor.Interact(
                inputPort,
                new CreateIngredientCategoryPresenter(mapper, this),
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
}
