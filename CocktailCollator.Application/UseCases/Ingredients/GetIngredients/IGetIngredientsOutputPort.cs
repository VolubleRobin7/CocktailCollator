using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.GetIngredients;

public interface IGetIngredientsOutputPort
{
    Task Success(List<Ingredient> ingredients, CancellationToken cancellationToken);
}
