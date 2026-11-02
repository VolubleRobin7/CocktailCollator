using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public interface IUpdateRecipeOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort, IExistenceOutputPort
{
    Task PersonalNoteUpdated(Recipe recipe, CancellationToken cancellationToken);

    Task RuleViolation(string failureReason, CancellationToken cancellationToken);

    Task Success(Recipe recipe, CancellationToken cancellationToken);
}
