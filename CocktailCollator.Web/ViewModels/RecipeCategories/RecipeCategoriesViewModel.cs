using AutoMapper;
using CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.RecipeCategories;

public class RecipeCategoriesViewModel
{
    public IAsyncRelayCommand<CreateRecipeCategoryInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }

    public List<RecipeCategoryViewModel> RecipeCategories { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public RecipeCategoriesViewModel(
        CreateRecipeCategoryInteractor createRecipeCategoryInteractor,
        DeleteRecipeCategoryInteractor deleteRecipeCategoryInteractor,
        GetRecipeCategoriesInteractor getRecipeCategoriesInteractor,
        IMapper mapper)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateRecipeCategoryInputPort>((inputPort, cancellationToken)
            => createRecipeCategoryInteractor.Interact(
                inputPort,
                new CreateRecipeCategoryPresenter(mapper, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((categoryId, cancellationToken)
            => deleteRecipeCategoryInteractor.Interact(
                new() { RecipeCategoryId = categoryId },
                new DeleteRecipeCategoryPresenter(this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getRecipeCategoriesInteractor.Interact(
                new GetRecipeCategoriesPresenter(mapper, this),
                cancellationToken));
    }

    private class CreateRecipeCategoryPresenter(IMapper mapper, RecipeCategoriesViewModel viewModel) : ICreateRecipeCategoryOutputPort
    {
        Task ICreateRecipeCategoryOutputPort.Success(RecipeCategory recipeCategory, CancellationToken cancellationToken)
        {
            viewModel.RecipeCategories.Add(mapper.Map<RecipeCategoryViewModel>(recipeCategory));
            return Task.CompletedTask;
        }
    }

    private class DeleteRecipeCategoryPresenter(RecipeCategoriesViewModel viewModel) : IDeleteRecipeCategoryOutputPort
    {
        Task IDeleteRecipeCategoryOutputPort.Failure(string reason, RecipeCategory? category, CancellationToken cancellationToken)
        {
            viewModel.Error = reason;
            return Task.CompletedTask;
        }

        Task IDeleteRecipeCategoryOutputPort.Success(RecipeCategory deletedCategory, CancellationToken cancellationToken)
        {
            _ = viewModel.RecipeCategories.RemoveAll(c => c.RecipeCategoryId == deletedCategory.RecipeCategoryId);
            return Task.CompletedTask;
        }
    }

    private class GetRecipeCategoriesPresenter(IMapper mapper, RecipeCategoriesViewModel viewModel) : IGetRecipeCategoriesOutputPort
    {
        Task IGetRecipeCategoriesOutputPort.Success(List<RecipeCategory> recipeCategories, CancellationToken cancellationToken)
        {
            viewModel.RecipeCategories = mapper.Map<List<RecipeCategoryViewModel>>(recipeCategories);
            return Task.CompletedTask;
        }
    }
}
