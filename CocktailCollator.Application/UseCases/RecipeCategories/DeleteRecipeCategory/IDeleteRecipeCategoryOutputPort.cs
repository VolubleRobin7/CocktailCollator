using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;

public interface IDeleteRecipeCategoryOutputPort
{
    Task Failure(string failureReason, RecipeCategory? recipeCategory, CancellationToken cancellationToken);
    Task Success(RecipeCategory recipeCategory, CancellationToken cancellationToken);
}
