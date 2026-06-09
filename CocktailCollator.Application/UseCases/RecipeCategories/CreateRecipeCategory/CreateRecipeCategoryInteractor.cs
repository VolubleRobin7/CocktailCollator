using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;

public class CreateRecipeCategoryInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(CreateRecipeCategoryInputPort inputPort, ICreateRecipeCategoryOutputPort outputPort, CancellationToken cancellationToken)
    {
        RecipeCategory _RecipeCategory = new() { Name = inputPort.Name };
        dbContext.Add(_RecipeCategory);
        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_RecipeCategory, cancellationToken);
    }
}
