using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.IngredientCategories.GetIngredientCategories;

public class GetIngredientCategoriesInteractor(ICocktailDbContext dbContext) : IInteractorPipe<IGetIngredientCategoriesOutputPort>
{
    public Task ExecuteAsync(IGetIngredientCategoriesOutputPort outputPort, CancellationToken cancellationToken)
        => outputPort.Success([.. dbContext.GetEntities<IngredientCategory>()], cancellationToken);
}
