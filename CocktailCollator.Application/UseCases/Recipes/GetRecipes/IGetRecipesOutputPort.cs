using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.GetRecipes;

public interface IGetRecipesOutputPort : IAuthenticatableOutputPort
{
    Task Success(List<Recipe> recipes, CancellationToken cancellationToken);
}
