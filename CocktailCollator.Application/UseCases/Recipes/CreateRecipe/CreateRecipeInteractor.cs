using AutoMapper;
using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeInteractor(ICocktailDbContext dbContext, IMapper mapper)
{
    public async Task Interact(CreateRecipeInputPort inputPort, ICreateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Ingredients = mapper.Map<List<CreateRecipeInputPortIngredient>, List<Ingredient>>(inputPort.Ingredients);
        var _RecipeIngredients = mapper.Map<List<Ingredient>, List<RecipeIngredient>>(_Ingredients);
        var _Recipe = mapper.Map<CreateRecipeInputPort, Recipe>(inputPort);
        _Recipe.Ingredients = _RecipeIngredients;

        dbContext.Add(_Recipe);

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Recipe, cancellationToken);
    }
}
