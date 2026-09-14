using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public interface IUpdateRecipeOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task NotFound(CancellationToken cancellationToken);

    Task Success(Recipe recipe, CancellationToken cancellationToken);
}
