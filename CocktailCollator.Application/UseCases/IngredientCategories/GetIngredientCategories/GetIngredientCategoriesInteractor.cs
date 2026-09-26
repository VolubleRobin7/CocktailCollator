using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.IngredientCategories.GetIngredientCategories;

public class GetIngredientCategoriesInteractor(ICocktailDbContext dbContext) : IInteractorPipe<GetIngredientCategoriesInputPort, IGetIngredientCategoriesOutputPort>
{
    public Task ExecuteAsync(GetIngredientCategoriesInputPort inputPort, IGetIngredientCategoriesOutputPort outputPort, CancellationToken cancellationToken)
        => outputPort.Success([.. dbContext.GetEntities<IngredientCategory>()], cancellationToken);
}
