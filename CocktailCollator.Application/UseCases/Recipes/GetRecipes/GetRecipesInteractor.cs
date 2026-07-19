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
                RecipeCategoryId = r.RecipeCategoryId,
                Category = r.Category,
                Ingredients = r.Ingredients!
                    .Select(ri => new RecipeIngredient
                    {
                        Amount = ri.Amount,
                        Ingredient = ri.Ingredient,
                        IngredientId = ri.IngredientId,
                        Measurement = ri.Measurement,
                        MeasurementId = ri.MeasurementId,
                        RecipeId = ri.RecipeId,
                    })
                    .ToList(),
                Steps = r.Steps,
                Images = r.Images!
                    .Select(ri => new RecipeDocument
                    {
                        RecipeId = ri.RecipeId,
                        DocumentId = ri.DocumentId,
                        Document = new Document
                        {
                            DocumentId = ri.Document.DocumentId,
                            FilePath = ri.Document.FilePath,
                            OriginalFileName = ri.Document.OriginalFileName,
                        }
                    }).ToList()
            });

        return outputPort.Success([.. _Recipes], cancellationToken);
    }
}
