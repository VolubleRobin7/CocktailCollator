using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public interface IUpdateRecipeOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort, IExistenceOutputPort
{
    Task Success(Recipe recipe, CancellationToken cancellationToken);
}
