using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.IngredientCategories.DeleteIngredientCategory;

public class DeleteIngredientCategoryInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(DeleteIngredientCategoryInputPort inputPort, IDeleteIngredientCategoryOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _IngredientCategory = dbContext.GetEntities<IngredientCategory>()
            .First(category => category.IngredientCategoryId == inputPort.IngredientCategoryId);

        if (dbContext.GetEntities<Ingredient>().Any(i => i.IngredientCategoryId == inputPort.IngredientCategoryId))
        {
            await outputPort.Failure("Ingredients are still using this category.", _IngredientCategory, cancellationToken);
            return;
        }

        dbContext.Remove(_IngredientCategory);

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_IngredientCategory, cancellationToken);
    }
}
