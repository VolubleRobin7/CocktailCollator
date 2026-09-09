using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public interface ICreateRecipeOutputPort : IAuthenticatableOutputPort
{
    Task Success(Recipe recipe, CancellationToken cancellationToken);
}
