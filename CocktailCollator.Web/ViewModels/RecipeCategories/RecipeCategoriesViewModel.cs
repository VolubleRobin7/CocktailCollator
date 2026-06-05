using CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;
using CocktailCollator.Application.UseCases.RecipeCategories.UpdateRecipeCategory;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.RecipeCategories;

public partial class RecipeCategoriesViewModel(
    CreateRecipeCategoryInteractor createInteractor,
    DeleteRecipeCategoryInteractor deleteInteractor,
    GetRecipeCategoriesInteractor getInteractor,
    UpdateRecipeCategoryInteractor updateInteractor
    ) : ObservableObject,
    ICreateRecipeCategoryOutputPort,
    IDeleteRecipeCategoryOutputPort,
    IGetRecipeCategoriesOutputPort,
    IUpdateRecipeCategoryOutputPort
{
    [ObservableProperty]
    private string _error = string.Empty;

    [ObservableProperty]
    private List<RecipeCategoryViewModel> _recipeCategories = [];

    [RelayCommand]
    public async Task CreateAsync(CreateRecipeCategoryInputPort inputPort)
    {
        this.Error = string.Empty;
        await createInteractor.Interact(inputPort, this, CancellationToken.None);
    }

    [RelayCommand]
    public async Task DeleteAsync(Guid recipeCategoryId)
    {
        this.Error = string.Empty;
        await deleteInteractor.Interact(new DeleteRecipeCategoryInputPort { RecipeCategoryId = recipeCategoryId }, this, CancellationToken.None);
    }

    [RelayCommand]
    public async Task GetAsync()
    {
        this.Error = string.Empty;
        await getInteractor.Interact(this, CancellationToken.None);
    }

    [RelayCommand]
    public async Task UpdateAsync(UpdateRecipeCategoryInputPort inputPort)
    {
        this.Error = string.Empty;
        await updateInteractor.Interact(inputPort, this, CancellationToken.None);
    }

    Task ICreateRecipeCategoryOutputPort.Success(RecipeCategory recipeCategory, CancellationToken cancellationToken)
    {
        this.RecipeCategories.Add(new RecipeCategoryViewModel
        {
            RecipeCategoryId = recipeCategory.RecipeCategoryId,
            Name = recipeCategory.Name
        });
        return Task.CompletedTask;
    }

    Task IDeleteRecipeCategoryOutputPort.Failure(string failureReason, RecipeCategory? recipeCategory, CancellationToken cancellationToken)
    {
        this.Error = failureReason;
        return Task.CompletedTask;
    }

    Task IDeleteRecipeCategoryOutputPort.Success(RecipeCategory recipeCategory, CancellationToken cancellationToken)
    {
        var categoryToRemove = this.RecipeCategories.FirstOrDefault(c => c.RecipeCategoryId == recipeCategory.RecipeCategoryId);
        if (categoryToRemove is not null)
            this.RecipeCategories.Remove(categoryToRemove);
        return Task.CompletedTask;
    }

    Task IGetRecipeCategoriesOutputPort.Success(List<RecipeCategory> recipeCategories, CancellationToken cancellationToken)
    {
        this.RecipeCategories = [.. recipeCategories.Select(category => new RecipeCategoryViewModel
        {
            RecipeCategoryId = category.RecipeCategoryId,
            Name = category.Name
        })];
        return Task.CompletedTask;
    }

    Task IUpdateRecipeCategoryOutputPort.Success(RecipeCategory recipeCategory, CancellationToken cancellationToken)
    {
        var categoryToUpdate = this.RecipeCategories.FirstOrDefault(c => c.RecipeCategoryId == recipeCategory.RecipeCategoryId);
        if (categoryToUpdate is not null)
            categoryToUpdate.Name = recipeCategory.Name;
        return Task.CompletedTask;
    }
}
