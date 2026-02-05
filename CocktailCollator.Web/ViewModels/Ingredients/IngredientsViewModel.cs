using AutoMapper;
using CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;
using CocktailCollator.Application.UseCases.Ingredients.GetIngredients;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Ingredients;

public class IngredientsViewModel
{
    public IAsyncRelayCommand<Guid> DeleteCommand { get; set; }
    public IAsyncRelayCommand GetCommand { get; set; }

    public List<IngredientViewModel> Ingredients { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public IngredientsViewModel(
        DeleteIngredientInteractor deleteIngredientInteractor,
        GetIngredientsInteractor getIngredientsInteractor,
        IMapper mapper)
    {
        this.DeleteCommand = new AsyncRelayCommand<Guid>((ingredientId, cancellationToken)
            => deleteIngredientInteractor.Interact(
                new() { IngredientId = ingredientId },
                new DeleteIngredientPresenter(this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getIngredientsInteractor.Interact(
                new GetIngredientsPresenter(mapper, this),
                cancellationToken));
    }

    private class DeleteIngredientPresenter(IngredientsViewModel viewModel) : IDeleteIngredientOutputPort
    {
        Task IDeleteIngredientOutputPort.Failure(string reason, Ingredient? ingredient, CancellationToken cancellationToken)
        {
            viewModel.Error = reason;
            return Task.CompletedTask;
        }

        Task IDeleteIngredientOutputPort.Success(Ingredient deletedIngredient, CancellationToken cancellationToken)
        {
            _ = viewModel.Ingredients.RemoveAll(ingredient => ingredient.IngredientId == deletedIngredient.IngredientId);
            return Task.CompletedTask;
        }
    }

    private class GetIngredientsPresenter(IMapper mapper, IngredientsViewModel viewModel) : IGetIngredientsOutputPort
    {
        Task IGetIngredientsOutputPort.Success(List<Ingredient> ingredients, CancellationToken cancellationToken)
        {
            viewModel.Ingredients = mapper.Map<List<IngredientViewModel>>(ingredients);
            return Task.CompletedTask;
        }
    }
}
