using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.GetRecipeNotes;

public class GetRecipeNotesInteractor(ICocktailDbContext dbContext)
{
    public Task InteractAsync(GetRecipeNotesInputPort inputPort, IGetRecipeNotesOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Query = dbContext.GetEntities<RecipeNote>()
            .Where(rn => rn.UserId == inputPort.UserId);

        return outputPort.Success([.. _Query.AsEnumerable()], cancellationToken);
    }
}
