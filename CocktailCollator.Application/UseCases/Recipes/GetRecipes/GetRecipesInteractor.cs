using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.GetRecipes;

public class GetRecipesInteractor(ICocktailDbContext dbContext)
{
    public Task Interact(IGetRecipesOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Query = dbContext.GetEntities<Recipe>()
            .Select(r => new
            {
                Recipe = r,
                r.Category,
                Ingredients = r.Ingredients!.Select(ri => new
                {
                    RecipeIngredient = ri,
                    ri.Ingredient,
                    ri.Measurement
                }),
                r.Steps,
                Images = r.Images!.Select(ri => new
                {
                    RecipeDocument = ri,
                    ri.Document
                })
            });

        return outputPort.Success([.. _Query.AsEnumerable().Select(x => x.Recipe)], cancellationToken);
    }
}
