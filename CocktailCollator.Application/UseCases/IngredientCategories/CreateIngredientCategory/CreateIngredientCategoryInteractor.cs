using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;

public class CreateIngredientCategoryInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(CreateIngredientCategoryInputPort inputPort, ICreateIngredientCategoryOutputPort outputPort, CancellationToken cancellationToken)
    {
        IngredientCategory _IngredientCategory = new() { Name = inputPort.Name };
        dbContext.Add(_IngredientCategory);
        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_IngredientCategory, cancellationToken);
    }
}
