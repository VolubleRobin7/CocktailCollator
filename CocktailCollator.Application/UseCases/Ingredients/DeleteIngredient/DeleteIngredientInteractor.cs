using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;

public class DeleteIngredientInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(DeleteIngredientInputPort inputPort, IDeleteIngredientOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Ingredient = dbContext.GetEntities<Ingredient>().First(ingredient => ingredient.IngredientId == inputPort.IngredientId);

        dbContext.Remove(_Ingredient);

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Ingredient, cancellationToken);
    }
}
