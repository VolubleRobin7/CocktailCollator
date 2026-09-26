using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;

public class GetRecipeCategoriesInteractor(ICocktailDbContext dbContext) : IInteractorPipe<IGetRecipeCategoriesOutputPort>
{
    public Task ExecuteAsync(IGetRecipeCategoriesOutputPort outputPort, CancellationToken cancellationToken)
        => outputPort.Success([.. dbContext.GetEntities<RecipeCategory>()], cancellationToken);
}
