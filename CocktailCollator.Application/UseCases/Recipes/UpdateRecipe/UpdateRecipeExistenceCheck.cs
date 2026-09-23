using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeExistenceCheck(ICocktailDbContext dbContext)
    : IExistencePipe<UpdateRecipeInputPort, IUpdateRecipeOutputPort>
{
    public async Task<bool> ExecuteAsync(UpdateRecipeInputPort inputPort, IUpdateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _RecipeExists = dbContext.GetEntities<Recipe>().Any(r => r.RecipeId == inputPort.RecipeId);

        if (!_RecipeExists)
        {
            await outputPort.NotFound(cancellationToken);
            return false;
        }
        else
            return true;
    }
}
