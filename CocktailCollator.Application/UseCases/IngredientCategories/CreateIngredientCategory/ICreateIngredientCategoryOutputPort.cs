using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;

public interface ICreateIngredientCategoryOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Success(IngredientCategory ingredientCategory, CancellationToken cancellationToken);
}
