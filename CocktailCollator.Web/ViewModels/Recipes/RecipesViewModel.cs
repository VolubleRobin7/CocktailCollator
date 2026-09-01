using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;
using CocktailCollator.Application.UseCases.Recipes.GetRecipeNotes;
using CocktailCollator.Application.UseCases.Recipes.GetRecipes;
using CocktailCollator.Application.UseCases.Recipes.SaveRecipeNote;
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
    public IAsyncRelayCommand<GetRecipeNotesInputPort> GetRecipeNotesCommand { get; set; }
    public IAsyncRelayCommand<UpdateRecipeInputPort> UpdateCommand { get; set; }
    public IAsyncRelayCommand<SaveRecipeNoteInputPort> SaveRecipeNoteCommand { get; set; }

    public List<RecipeViewModel> Recipes { get; private set; } = [];


    public RecipesViewModel(
        CreateRecipeInteractor createRecipeInteractor,
        DeleteRecipeInteractor deleteRecipeInteractor,
        GetRecipesInteractor getRecipesInteractor,
        GetRecipeNotesInteractor getRecipeNotesInteractor,
        UpdateRecipeInteractor updateRecipeInteractor,
        SaveRecipeNoteInteractor saveRecipeNoteInteractor,
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

        this.GetRecipeNotesCommand = new AsyncRelayCommand<GetRecipeNotesInputPort>((inputPort, cancellationToken)
            => getRecipeNotesInteractor.InteractAsync(
                inputPort,
                new GetRecipeNotesPresenter(mapper, store),
                cancellationToken));

        this.UpdateCommand = new AsyncRelayCommand<UpdateRecipeInputPort>((inputPort, cancellationToken)
            => updateRecipeInteractor.InteractAsync(
                inputPort,
                new UpdateRecipePresenter(mapper, store, toastService),
                cancellationToken));

        this.SaveRecipeNoteCommand = new AsyncRelayCommand<SaveRecipeNoteInputPort>((inputPort, cancellationToken)
            => saveRecipeNoteInteractor.InteractAsync(
                inputPort,
                new SaveRecipeNotePresenter(mapper, store, toastService),
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

    private class GetRecipeNotesPresenter(IMapper mapper, IViewModelStore store) : IGetRecipeNotesOutputPort
    {
        Task IGetRecipeNotesOutputPort.Success(List<RecipeNote> recipeNotes, CancellationToken cancellationToken)
        {
            foreach (var rn in recipeNotes)
            {
                var _RecipeViewModel = store.Get<RecipeViewModel>(rn.RecipeId);
                if (_RecipeViewModel != null)
                {
                    _RecipeViewModel.RecipeNotes ??= [];
                    var _NoteViewModel = mapper.Map<RecipeNotes.RecipeNoteViewModel>(rn);

                    var existingNoteIndex = _RecipeViewModel.RecipeNotes.FindIndex(r => r.RecipeNoteId == rn.RecipeNoteId);
                    if (existingNoteIndex >= 0)
                    {
                        _RecipeViewModel.RecipeNotes[existingNoteIndex] = store.UpdateOrRegister(_NoteViewModel.RecipeNoteId, _NoteViewModel);
                    }
                    else
                    {
                        _RecipeViewModel.RecipeNotes.Add(store.UpdateOrRegister(_NoteViewModel.RecipeNoteId, _NoteViewModel));
                    }
                }
            }
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

    private class SaveRecipeNotePresenter(IMapper mapper, IViewModelStore store, ToastService toastService) : ISaveRecipeNoteOutputPort
    {
        Task ISaveRecipeNoteOutputPort.Failure(string failureReason, CancellationToken cancellationToken)
        {
            toastService.ShowToast(ToastType.Danger, "Failed to Save Note", failureReason);
            return Task.CompletedTask;
        }

        Task ISaveRecipeNoteOutputPort.Success(RecipeNote recipeNote, CancellationToken cancellationToken)
        {
            var _RecipeViewModel = store.Get<RecipeViewModel>(recipeNote.RecipeId);
            if (_RecipeViewModel != null)
            {
                _RecipeViewModel.RecipeNotes ??= [];
                var _NoteViewModel = mapper.Map<RecipeNotes.RecipeNoteViewModel>(recipeNote);

                var existingNoteIndex = _RecipeViewModel.RecipeNotes.FindIndex(rn => rn.UserId == _NoteViewModel.UserId);
                if (existingNoteIndex >= 0)
                {
                    _RecipeViewModel.RecipeNotes[existingNoteIndex] = store.UpdateOrRegister(_NoteViewModel.RecipeNoteId, _NoteViewModel);
                }
                else
                {
                    _RecipeViewModel.RecipeNotes.Add(store.UpdateOrRegister(_NoteViewModel.RecipeNoteId, _NoteViewModel));
                }
            }
            toastService.ShowToast(ToastType.Success, "Note Saved", "Your personal note has been saved successfully");
            return Task.CompletedTask;
        }
    }
}
