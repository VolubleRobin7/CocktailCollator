using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;

public interface ICreateIngredientCategoryOutputPort
{
    Task Success(IngredientCategory ingredientCategory, CancellationToken cancellationToken);
}
