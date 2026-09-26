using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;

public class CreateIngredientInteractor(ICocktailDbContext dbContext) : IInteractorPipe<CreateIngredientInputPort, ICreateIngredientOutputPort>
{
    public async Task ExecuteAsync(CreateIngredientInputPort inputPort, ICreateIngredientOutputPort outputPort, CancellationToken cancellationToken)
    {
        Ingredient _Ingredient = new() { Name = inputPort.Name };
        dbContext.Add(_Ingredient);
        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Ingredient, cancellationToken);
    }
}
