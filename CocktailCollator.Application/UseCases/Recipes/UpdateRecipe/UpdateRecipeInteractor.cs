using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeInteractor(ICocktailDbContext dbContext)
{
    public async Task InteractAsync(UpdateRecipeInputPort inputPort, IUpdateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Recipe = dbContext.GetEntities<Recipe>().First(r => r.RecipeId == inputPort.RecipeId);

        // Details
        _Recipe.Name = inputPort.Name;
        _Recipe.RecipeCategoryId = inputPort.RecipeCategoryId;

        // Steps
        _Recipe.Steps = [.. inputPort.Steps.Select(s => new RecipeStep
        {
            Instruction = s.Instruction,
            Order = s.Order
        })];

        // Ingredients
        // This is a very inefficient way to update the recipe ingredients
        var _RecipeIngredients = dbContext.GetEntities<RecipeIngredient>().Where(ri => ri.RecipeId == inputPort.RecipeId);
        foreach (var recipeIngredient in _RecipeIngredients)
            dbContext.Remove(recipeIngredient);

        var newRecipeIngredients = new List<RecipeIngredient>();
        foreach (var recipeIngredient in inputPort.Ingredients)
        {
            if (recipeIngredient.Ingredient is not null)
            {
                var newRecipeIngredient = new RecipeIngredient
                {
                    Ingredient = new Ingredient
                    {
                        Name = recipeIngredient.Ingredient.Name,
                        Measurements = [new() { MeasurementId = recipeIngredient.MeasurementId }],
                    },
                    MeasurementId = recipeIngredient.MeasurementId,
                    Amount = recipeIngredient.Amount
                };
                newRecipeIngredients.Add(newRecipeIngredient);
            }
            else
            {
                var newRecipeIngredient = new RecipeIngredient
                {
                    RecipeId = _Recipe.RecipeId,
                    IngredientId = recipeIngredient.IngredientId,
                    MeasurementId = recipeIngredient.MeasurementId,
                    Amount = recipeIngredient.Amount
                };
                newRecipeIngredients.Add(newRecipeIngredient);
            }
        }
        _Recipe.Ingredients = newRecipeIngredients;

        // Images
        var _DocumentsToKeep = inputPort.Images
            .Where(i => i.ExistingDocumentId.HasValue)
            .Select(i => i.ExistingDocumentId!.Value);

        var _DocumentsToRemove = dbContext.GetEntities<RecipeDocument>()
            .Where(rd => rd.RecipeId == inputPort.RecipeId && !_DocumentsToKeep.Contains(rd.DocumentId));

        foreach (var _RecipeDocument in _DocumentsToRemove)
        {
            dbContext.Remove(_RecipeDocument);
            dbContext.QueueRemoveDocument(_RecipeDocument.DocumentId);
        }

        var _NewDocuments = inputPort.Images
            .Where(i => i.NewDocument is not null)
            .Select(i => i.NewDocument!);

        foreach (var _NewDocument in _NewDocuments)
        {
            var _NewDocumentId = dbContext.QueueAddDocument(_NewDocument, _Recipe, cancellationToken);
            _Recipe.Images ??= [];
            _Recipe.Images.Add(new RecipeDocument
            {
                RecipeId = _Recipe.RecipeId,
                DocumentId = _NewDocumentId,
            });
        }

        // Finalisation
        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Recipe, cancellationToken);
    }
}
