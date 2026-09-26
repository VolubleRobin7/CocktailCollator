using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;

public interface IDeleteRecipeCategoryOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Failure(string failureReason, RecipeCategory? recipeCategory, CancellationToken cancellationToken);
    Task Success(RecipeCategory recipeCategory, CancellationToken cancellationToken);
}
