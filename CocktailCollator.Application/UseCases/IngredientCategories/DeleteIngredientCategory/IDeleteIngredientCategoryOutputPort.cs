using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.IngredientCategories.DeleteIngredientCategory;

public interface IDeleteIngredientCategoryOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Failure(string failureReason, IngredientCategory? ingredientCategory, CancellationToken cancellationToken);

    Task Success(IngredientCategory ingredientCategory, CancellationToken cancellationToken);
}
