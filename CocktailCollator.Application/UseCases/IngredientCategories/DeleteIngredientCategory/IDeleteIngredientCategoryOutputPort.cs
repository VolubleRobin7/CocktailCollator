using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.IngredientCategories.DeleteIngredientCategory;

public interface IDeleteIngredientCategoryOutputPort
{
    Task Failure(string failureReason, IngredientCategory? ingredientCategory, CancellationToken cancellationToken);

    Task Success(IngredientCategory ingredientCategory, CancellationToken cancellationToken);
}
