using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;

public class UpdateIngredientInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(UpdateIngredientInputPort inputPort, IUpdateIngredientOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Ingredient = dbContext.GetEntities<Ingredient>().First(i => i.IngredientId == inputPort.IngredientId);

        _Ingredient.Name = inputPort.Name;
        _Ingredient.IngredientCategoryId = inputPort.IngredientCategoryId;

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Ingredient, cancellationToken);
    }
}
