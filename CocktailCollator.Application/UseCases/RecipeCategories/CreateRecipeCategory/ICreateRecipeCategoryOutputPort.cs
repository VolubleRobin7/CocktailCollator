using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;

public interface ICreateRecipeCategoryOutputPort
{
    Task Success(RecipeCategory recipeCategory, CancellationToken cancellationToken);
}
