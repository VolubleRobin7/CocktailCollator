using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public interface IUpdateRecipeOutputPort
{
    Task NotFound(CancellationToken cancellationToken);

    Task Success(Recipe recipe, CancellationToken cancellationToken);

    Task Unauthorised(CancellationToken cancellationToken);
}
