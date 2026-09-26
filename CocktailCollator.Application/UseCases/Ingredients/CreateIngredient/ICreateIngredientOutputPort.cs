using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;

public interface ICreateIngredientOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Success(Ingredient ingredient, CancellationToken cancellationToken);
}
