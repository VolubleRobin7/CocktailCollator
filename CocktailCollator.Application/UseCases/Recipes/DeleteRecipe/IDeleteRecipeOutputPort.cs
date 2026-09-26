using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;

public interface IDeleteRecipeOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Success(Recipe recipe, CancellationToken cancellationToken);
}
