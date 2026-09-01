using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.GetRecipeNotes;

public interface IGetRecipeNotesOutputPort
{
    Task Success(List<RecipeNote> recipeNotes, CancellationToken cancellationToken);
}
