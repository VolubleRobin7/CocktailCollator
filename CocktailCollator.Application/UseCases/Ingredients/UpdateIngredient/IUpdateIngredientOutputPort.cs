using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;

public interface IUpdateIngredientOutputPort
{
    Task Failure(string failureReason, Ingredient? ingredient, CancellationToken cancellationToken);

    Task Success(Ingredient ingredient, CancellationToken cancellationToken);
}
