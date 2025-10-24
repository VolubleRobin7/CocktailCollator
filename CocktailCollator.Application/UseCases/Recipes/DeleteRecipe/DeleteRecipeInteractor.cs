using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;

public class DeleteRecipeInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(DeleteRecipeInputPort inputPort, IDeleteRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Recipe = dbContext.GetEntities<Recipe>().First(recipe => recipe.RecipeId == inputPort.RecipeId);

        dbContext.Remove(_Recipe);

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Recipe, cancellationToken);
    }
}
