using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;

public interface IUpdateIngredientOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort, IExistenceOutputPort
{
    Task Success(Ingredient ingredient, CancellationToken cancellationToken);
}
