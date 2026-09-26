using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;

public class UpdateIngredientExistenceCheck(ICocktailDbContext dbContext)
    : IExistencePipe<UpdateIngredientInputPort, IUpdateIngredientOutputPort>
{
    public async Task<bool> ExecuteAsync(UpdateIngredientInputPort inputPort, IUpdateIngredientOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _IngredientExists = dbContext.GetEntities<Ingredient>().Any(i => i.IngredientId == inputPort.IngredientId);

        if (!_IngredientExists)
        {
            await outputPort.NotFound(cancellationToken);
            return false;
        }
        else
            return true;
    }
}
