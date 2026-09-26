using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.IngredientCategories.GetIngredientCategories;

public interface IGetIngredientCategoriesOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Success(List<IngredientCategory> ingredientCategories, CancellationToken cancellationToken);
}
