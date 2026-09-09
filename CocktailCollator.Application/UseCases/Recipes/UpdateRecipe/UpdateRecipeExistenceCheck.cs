using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Infrastructure;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeExistenceCheck(IPipeline<UpdateRecipeInputPort, IUpdateRecipeOutputPort> innerPipe, ICocktailDbContext dbContext)
    : IExistencePipe<UpdateRecipeInputPort, IUpdateRecipeOutputPort>
{
    public IPipeline<UpdateRecipeInputPort, IUpdateRecipeOutputPort> InnerPipe => innerPipe;

    public Task ExecuteAsync(UpdateRecipeInputPort inputPort, IUpdateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _RecipeExists = dbContext.GetEntities<Recipe>().Any(r => r.RecipeId == inputPort.RecipeId);

        if (!_RecipeExists)
            return outputPort.NotFound(cancellationToken);
        else
            return innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
    }
}
