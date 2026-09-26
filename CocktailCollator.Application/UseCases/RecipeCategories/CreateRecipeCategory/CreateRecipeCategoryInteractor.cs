using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;

public class CreateRecipeCategoryInteractor(ICocktailDbContext dbContext) : IInteractorPipe<CreateRecipeCategoryInputPort, ICreateRecipeCategoryOutputPort>
{
    public async Task ExecuteAsync(CreateRecipeCategoryInputPort inputPort, ICreateRecipeCategoryOutputPort outputPort, CancellationToken cancellationToken)
    {
        RecipeCategory _RecipeCategory = new() { Name = inputPort.Name };
        dbContext.Add(_RecipeCategory);
        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_RecipeCategory, cancellationToken);
    }
}
