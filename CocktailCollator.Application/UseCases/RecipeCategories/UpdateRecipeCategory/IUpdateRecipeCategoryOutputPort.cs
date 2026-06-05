using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.UpdateRecipeCategory;

public interface IUpdateRecipeCategoryOutputPort
{
    Task Success(RecipeCategory recipeCategory, CancellationToken cancellationToken);
}
