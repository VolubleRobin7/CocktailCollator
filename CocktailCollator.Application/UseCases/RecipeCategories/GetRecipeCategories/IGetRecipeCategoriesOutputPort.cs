using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;

public interface IGetRecipeCategoriesOutputPort
{
    Task Success(List<RecipeCategory> recipeCategories, CancellationToken cancellationToken);
}
