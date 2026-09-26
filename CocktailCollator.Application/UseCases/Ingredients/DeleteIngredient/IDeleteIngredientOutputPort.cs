using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;

public interface IDeleteIngredientOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Failure(string failureReason, Ingredient? ingredient, CancellationToken cancellationToken);

    Task Success(Ingredient ingredient, CancellationToken cancellationToken);
}
