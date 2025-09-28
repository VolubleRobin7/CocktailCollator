using AutoMapper;
using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeInteractor(ICocktailDbContext dbContext, IMapper mapper)
{
    public async Task Interact(CreateRecipeInputPort inputPort, ICreateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Recipe = mapper.Map<Recipe>(inputPort);

        dbContext.Add(_Recipe);

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Recipe, cancellationToken);
    }
}
