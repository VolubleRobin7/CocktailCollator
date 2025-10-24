using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;

public interface IDeleteRecipeOutputPort
{
    Task Success(Recipe recipe, CancellationToken cancellationToken);
}
