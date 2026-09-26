using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.GetIngredients;

public interface IGetIngredientsOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Success(List<Ingredient> ingredients, CancellationToken cancellationToken);
}
