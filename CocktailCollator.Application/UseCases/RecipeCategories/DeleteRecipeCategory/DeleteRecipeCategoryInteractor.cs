using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;

public class DeleteRecipeCategoryInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(DeleteRecipeCategoryInputPort inputPort, IDeleteRecipeCategoryOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _RecipeCategory = dbContext.GetEntities<RecipeCategory>()
            .First(category => category.RecipeCategoryId == inputPort.RecipeCategoryId);

        if (dbContext.GetEntities<Recipe>().Any(i => i.RecipeCategoryId == inputPort.RecipeCategoryId))
        {
            await outputPort.Failure("Recipes are still using this category.", _RecipeCategory, cancellationToken);
            return;
        }

        dbContext.Remove(_RecipeCategory);
        await dbContext.SaveChangesAsync(cancellationToken);

        await outputPort.Success(_RecipeCategory, cancellationToken);
    }
}
