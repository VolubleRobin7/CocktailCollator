using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;

public interface IGetRecipeCategoriesOutputPort : IAuthenticatableOutputPort
{
    Task Success(List<RecipeCategory> recipeCategories, CancellationToken cancellationToken);
}
