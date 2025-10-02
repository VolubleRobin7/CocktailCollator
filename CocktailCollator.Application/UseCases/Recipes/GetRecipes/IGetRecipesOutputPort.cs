using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.GetRecipes;

public interface IGetRecipesOutputPort
{
    Task Success(List<Recipe> recipes, CancellationToken cancellationToken);
}
