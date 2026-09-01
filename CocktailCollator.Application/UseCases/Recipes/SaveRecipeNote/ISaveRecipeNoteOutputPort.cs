using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.SaveRecipeNote;

public interface ISaveRecipeNoteOutputPort
{
    Task Success(RecipeNote recipeNote, CancellationToken cancellationToken);
    Task Failure(string error, CancellationToken cancellationToken);
}
