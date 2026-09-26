using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;

public class GetRecipeCategoriesInteractor(ICocktailDbContext dbContext) : IInteractorPipe<GetRecipeCategoriesInputPort, IGetRecipeCategoriesOutputPort>
{
    public Task ExecuteAsync(GetRecipeCategoriesInputPort inputPort, IGetRecipeCategoriesOutputPort outputPort, CancellationToken cancellationToken)
        => outputPort.Success([.. dbContext.GetEntities<RecipeCategory>()], cancellationToken);
}
