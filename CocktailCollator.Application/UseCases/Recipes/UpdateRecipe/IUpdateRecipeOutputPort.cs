using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public interface IUpdateRecipeOutputPort
{
    Task Failure(string failureReason, Recipe? recipe, CancellationToken cancellationToken);

    Task Success(Recipe recipe, CancellationToken cancellationToken);
}
