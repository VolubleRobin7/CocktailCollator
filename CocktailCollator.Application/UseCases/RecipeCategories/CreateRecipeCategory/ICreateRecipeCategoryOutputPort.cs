using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;

public interface ICreateRecipeCategoryOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Success(RecipeCategory recipeCategory, CancellationToken cancellationToken);
}
