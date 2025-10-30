using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;

public class DeleteIngredientInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(DeleteIngredientInputPort inputPort, IDeleteIngredientOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Ingredient = dbContext.GetEntities<Ingredient>().First(ingredient => ingredient.IngredientId == inputPort.IngredientId);

        if (dbContext.GetEntities<Recipe>().Any(r => r.Ingredients!.Any(ri => ri.IngredientId == inputPort.IngredientId)))
        {
            await outputPort.Failure("Recipes are still using this ingredient.", _Ingredient, cancellationToken);
            return;
        }

        dbContext.Remove(_Ingredient);

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Ingredient, cancellationToken);
    }
}
