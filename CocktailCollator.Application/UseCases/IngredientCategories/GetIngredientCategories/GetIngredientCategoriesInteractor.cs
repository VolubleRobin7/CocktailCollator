using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.IngredientCategories.GetIngredientCategories;

public class GetIngredientCategoriesInteractor(ICocktailDbContext dbContext)
{
    public Task Interact(IGetIngredientCategoriesOutputPort outputPort, CancellationToken cancellationToken)
        => outputPort.Success([.. dbContext.GetEntities<IngredientCategory>()], cancellationToken);
}
