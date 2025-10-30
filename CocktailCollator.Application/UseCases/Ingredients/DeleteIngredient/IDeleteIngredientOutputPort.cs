using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;

public interface IDeleteIngredientOutputPort
{
    Task Success(Ingredient ingredient, CancellationToken cancellationToken);
}
