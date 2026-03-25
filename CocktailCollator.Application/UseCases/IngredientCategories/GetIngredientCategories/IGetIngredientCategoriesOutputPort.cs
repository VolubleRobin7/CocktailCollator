using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.IngredientCategories.GetIngredientCategories;

public interface IGetIngredientCategoriesOutputPort
{
    Task Success(List<IngredientCategory> ingredientCategories, CancellationToken cancellationToken);
}
