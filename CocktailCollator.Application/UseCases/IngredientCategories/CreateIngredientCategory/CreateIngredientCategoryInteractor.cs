using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;

public class CreateIngredientCategoryInteractor(ICocktailDbContext dbContext) : IInteractorPipe<CreateIngredientCategoryInputPort, ICreateIngredientCategoryOutputPort>
{
    public async Task ExecuteAsync(CreateIngredientCategoryInputPort inputPort, ICreateIngredientCategoryOutputPort outputPort, CancellationToken cancellationToken)
    {
        IngredientCategory _IngredientCategory = new() { Name = inputPort.Name };
        dbContext.Add(_IngredientCategory);
        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_IngredientCategory, cancellationToken);
    }
}
