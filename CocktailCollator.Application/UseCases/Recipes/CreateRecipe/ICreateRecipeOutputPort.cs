using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public interface ICreateRecipeOutputPort
{
    Task Success(Recipe recipe, CancellationToken cancellationToken);
}
