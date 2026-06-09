using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;

public class GetRecipeCategoriesInteractor(ICocktailDbContext dbContext)
{
    public Task Interact(IGetRecipeCategoriesOutputPort outputPort, CancellationToken cancellationToken)
        => outputPort.Success([.. dbContext.GetEntities<RecipeCategory>()], cancellationToken);
}
