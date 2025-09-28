using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;

public interface ICreateIngredientOutputPort
{
    Task Success(Ingredient ingredient, CancellationToken cancellationToken);
}
