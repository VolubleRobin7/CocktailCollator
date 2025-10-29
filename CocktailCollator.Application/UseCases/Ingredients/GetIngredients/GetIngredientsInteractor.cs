using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.GetIngredients;

public class GetIngredientsInteractor(ICocktailDbContext dbContext)
{
    public Task Interact(IGetIngredientsOutputPort outputPort, CancellationToken cancellationToken)
        => outputPort.Success([.. dbContext.GetEntities<Ingredient>()], cancellationToken);
}
