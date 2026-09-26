using AutoMapper;
using CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;
using CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;
using CocktailCollator.Application.UseCases.Ingredients.GetIngredients;
using CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;
using CocktailCollator.Web.Common.Presenters;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.Common.State;
using CocktailCollator.Web.Views.Components.Toasts;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Ingredients;

public class IngredientsViewModel
{
    public IAsyncRelayCommand<CreateIngredientInputPort>? CreateCommand { get; set; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; set; }
    public IAsyncRelayCommand GetCommand { get; set; }
    public IAsyncRelayCommand<UpdateIngredientInputPort> UpdateCommand { get; set; }

    public List<IngredientViewModel> Ingredients { get; private set; } = [];


    public IngredientsViewModel(
        IPipeline<CreateIngredientInputPort, ICreateIngredientOutputPort> createIngredientPipeline,
        IPipeline<DeleteIngredientInputPort, IDeleteIngredientOutputPort> deleteIngredientPipeline,
        IPipeline<IGetIngredientsOutputPort> getIngredientsPipeline,
        IPipeline<UpdateIngredientInputPort, IUpdateIngredientOutputPort> updateIngredientPipeline,
        IMapper mapper,
        IViewModelStore store,
        ToastService toastService)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateIngredientInputPort>((inputPort, cancellationToken)
            => createIngredientPipeline.ExecuteAsync(
                inputPort,
                new CreateIngredientPresenter(mapper, store, toastService, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((ingredientId, cancellationToken)
            => deleteIngredientPipeline.ExecuteAsync(
                new() { IngredientId = ingredientId },
                new DeleteIngredientPresenter(store, toastService, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getIngredientsPipeline.ExecuteAsync(
                new GetIngredientsPresenter(mapper, store, toastService, this),
                cancellationToken));

        this.UpdateCommand = new AsyncRelayCommand<UpdateIngredientInputPort>((inputPort, cancellationToken)
            => updateIngredientPipeline.ExecuteAsync(
                inputPort,
                new UpdateIngredientPresenter(mapper, store, toastService),
                cancellationToken));
    }

    private class CreateIngredientPresenter(IMapper mapper, IViewModelStore store, ToastService toastService, IngredientsViewModel viewModel)
        : BasePresenter(toastService, "create ingredients"), ICreateIngredientOutputPort
    {
        Task ICreateIngredientOutputPort.Success(Ingredient ingredient, CancellationToken cancellationToken)
        {
            var _Ingredient = mapper.Map<IngredientViewModel>(ingredient);
            viewModel.Ingredients.Add(store.UpdateOrRegister(_Ingredient.IngredientId, _Ingredient));
            this.ToastService.ShowToast(ToastType.Success, "Ingredient Created", $"{ingredient.Name} created successfully");
            return Task.CompletedTask;
        }
    }

    private class DeleteIngredientPresenter(IViewModelStore store, ToastService toastService, IngredientsViewModel viewModel)
        : BasePresenter(toastService, "delete ingredients"), IDeleteIngredientOutputPort
    {
        Task IDeleteIngredientOutputPort.StillInUse(string reason, Ingredient? ingredient, CancellationToken cancellationToken)
        {
            this.ToastService.ShowToast(ToastType.Danger, "Failed to Delete", reason);
            return Task.CompletedTask;
        }

        Task IDeleteIngredientOutputPort.Success(Ingredient deletedIngredient, CancellationToken cancellationToken)
        {
            _ = viewModel.Ingredients.RemoveAll(ingredient => ingredient.IngredientId == deletedIngredient.IngredientId);
            store.Remove<IngredientViewModel>(deletedIngredient.IngredientId);
            this.ToastService.ShowToast(ToastType.Info, "Ingredient Deleted", $"{deletedIngredient.Name} deleted successfully");
            return Task.CompletedTask;
        }
    }

    private class GetIngredientsPresenter(IMapper mapper, IViewModelStore store, ToastService toastService, IngredientsViewModel viewModel)
        : BasePresenter(toastService, "view ingredients"), IGetIngredientsOutputPort
    {
        Task IGetIngredientsOutputPort.Success(List<Ingredient> ingredients, CancellationToken cancellationToken)
        {
            viewModel.Ingredients = [.. mapper.Map<List<IngredientViewModel>>(ingredients).Select(i => store.UpdateOrRegister(i.IngredientId, i))];
            return Task.CompletedTask;
        }
    }

    private class UpdateIngredientPresenter(IMapper mapper, IViewModelStore store, ToastService toastService)
        : BasePresenter(toastService, "update ingredients"), IUpdateIngredientOutputPort
    {
        public override Task NotFound(CancellationToken cancellationToken)
        {
            this.ToastService.ShowToast(ToastType.Warning, "Ingredient Not Found", "The ingredient you are trying to update does not exist");
            return Task.CompletedTask;
        }

        Task IUpdateIngredientOutputPort.Success(Ingredient ingredient, CancellationToken cancellationToken)
        {
            var _Ingredient = mapper.Map<IngredientViewModel>(ingredient);
            _ = store.UpdateOrRegister(_Ingredient.IngredientId, _Ingredient);
            this.ToastService.ShowToast(ToastType.Success, "Ingredient Updated", $"{ingredient.Name} updated successfully");
            return Task.CompletedTask;
        }
    }
}
