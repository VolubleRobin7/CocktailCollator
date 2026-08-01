using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;

public class DeleteRecipeInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(DeleteRecipeInputPort inputPort, IDeleteRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        // Returns a tuple of Recipe and RecipeDocument
        var _RecipeData = dbContext.GetEntities<Recipe>()
            .Where(recipe => recipe.RecipeId == inputPort.RecipeId)
            .Select(recipe => new { Recipe = recipe, recipe.Images })
            .First();

        if (_RecipeData.Images is not null)
        {
            foreach (var _Image in _RecipeData.Images)
                dbContext.QueueRemoveDocument(_Image.DocumentId);
        }

        dbContext.Remove(_RecipeData.Recipe);

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_RecipeData.Recipe, cancellationToken);
    }
}
