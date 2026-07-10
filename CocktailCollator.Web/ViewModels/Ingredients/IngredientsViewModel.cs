using AutoMapper;
using CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;
using CocktailCollator.Application.UseCases.Ingredients.GetIngredients;
using CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;
using CocktailCollator.Domain.Entities;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.Views.Components.Toasts;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Ingredients;

public class IngredientsViewModel
{
    public IAsyncRelayCommand<Guid> DeleteCommand { get; set; }
    public IAsyncRelayCommand GetCommand { get; set; }
    public IAsyncRelayCommand<UpdateIngredientInputPort> UpdateCommand { get; set; }

    public List<IngredientViewModel> Ingredients { get; private set; } = [];


    public IngredientsViewModel(
        DeleteIngredientInteractor deleteIngredientInteractor,
        GetIngredientsInteractor getIngredientsInteractor,
        UpdateIngredientInteractor updateIngredientInteractor,
        IMapper mapper,
        ToastService toastService)
    {
        this.DeleteCommand = new AsyncRelayCommand<Guid>((ingredientId, cancellationToken)
            => deleteIngredientInteractor.Interact(
                new() { IngredientId = ingredientId },
                new DeleteIngredientPresenter(toastService, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getIngredientsInteractor.Interact(
                new GetIngredientsPresenter(mapper, this),
                cancellationToken));

        this.UpdateCommand = new AsyncRelayCommand<UpdateIngredientInputPort>((inputPort, cancellationToken)
            => updateIngredientInteractor.Interact(
                inputPort,
                new UpdateIngredientPresenter(mapper, toastService, this),
                cancellationToken));
    }

    private class DeleteIngredientPresenter(ToastService toastService, IngredientsViewModel viewModel) : IDeleteIngredientOutputPort
    {
        Task IDeleteIngredientOutputPort.Failure(string reason, Ingredient? ingredient, CancellationToken cancellationToken)
        {
            toastService.ShowToast(ToastType.Danger, "Failed to Delete", reason);
            return Task.CompletedTask;
        }

        Task IDeleteIngredientOutputPort.Success(Ingredient deletedIngredient, CancellationToken cancellationToken)
        {
            _ = viewModel.Ingredients.RemoveAll(ingredient => ingredient.IngredientId == deletedIngredient.IngredientId);
            toastService.ShowToast(ToastType.Info, "Ingredient Deleted", $"{deletedIngredient.Name} deleted successfully");
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

    private class UpdateIngredientPresenter(IMapper mapper, ToastService toastService, IngredientsViewModel viewModel) : IUpdateIngredientOutputPort
    {
        Task IUpdateIngredientOutputPort.Failure(string failureReason, Ingredient? ingredient, CancellationToken cancellationToken)
        {
            toastService.ShowToast(ToastType.Danger, "Failed to Update", failureReason);
            return Task.CompletedTask;
        }

        Task IUpdateIngredientOutputPort.Success(Ingredient ingredient, CancellationToken cancellationToken)
        {
            var _Existing = viewModel.Ingredients.FirstOrDefault(i => i.IngredientId == ingredient.IngredientId);
            if (_Existing is not null)
            {
                var _Updated = mapper.Map<IngredientViewModel>(ingredient);
                _Existing.Name = _Updated.Name;
                _Existing.Measurements = _Updated.Measurements;
                _Existing.Category = _Updated.Category;
            }
            toastService.ShowToast(ToastType.Success, "Ingredient Updated", $"{ingredient.Name} updated successfully");
            return Task.CompletedTask;
        }
    }
}
