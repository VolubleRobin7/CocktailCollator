using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;

public class DeleteRecipeInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(DeleteRecipeInputPort inputPort, IDeleteRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Images = dbContext.GetEntities<RecipeDocument>().Where(recipe => recipe.RecipeId == inputPort.RecipeId);

        foreach (var _Image in _Images)
            dbContext.QueueRemoveDocument(_Image.DocumentId);

        var _Recipe = dbContext.GetEntities<Recipe>().First(recipe => recipe.RecipeId == inputPort.RecipeId);

        dbContext.Remove(_Recipe);

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Recipe, cancellationToken);
    }
}
