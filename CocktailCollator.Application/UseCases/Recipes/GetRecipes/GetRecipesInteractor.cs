using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.GetRecipes;

public class GetRecipesInteractor(ICocktailDbContext dbContext)
{
    public Task Interact(IGetRecipesOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Recipes = dbContext.GetEntities<Recipe>()
            .Select(r => new Recipe()
            {
                RecipeId = r.RecipeId,
                Name = r.Name,
                Ingredients = r.Ingredients!
                    .Select(ri => new RecipeIngredient
                    {
                        RecipeId = ri.RecipeId,
                        IngredientId = ri.IngredientId,
                        Ingredient = ri.Ingredient
                    })
                    .ToList(),
                Steps = r.Steps
            });

        return outputPort.Success([.. _Recipes], cancellationToken);
    }
}
