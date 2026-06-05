using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.UpdateRecipeCategory;

public class UpdateRecipeCategoryInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(UpdateRecipeCategoryInputPort inputPort, IUpdateRecipeCategoryOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _RecipeCategory = dbContext.GetEntities<RecipeCategory>().First(i => i.RecipeCategoryId == inputPort.RecipeCategoryId);

        _RecipeCategory.Name = inputPort.Name;

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_RecipeCategory, cancellationToken);
    }
}
