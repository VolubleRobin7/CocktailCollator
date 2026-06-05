using AutoMapper;
using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeInteractor(ICocktailDbContext dbContext)
{
    public async Task InteractAsync(UpdateRecipeInputPort inputPort, IUpdateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Recipe = dbContext.GetEntities<Recipe>().First(r => r.RecipeId == inputPort.RecipeId);

        _Recipe.Name = inputPort.Name;
        _Recipe.RecipeCategoryId = inputPort.RecipeCategoryId;

        _Recipe.Steps = [.. inputPort.Steps.Select(s => new RecipeStep
        {
            Instruction = s.Instruction,
            Order = s.Order
        })];

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

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Recipe, cancellationToken);
    }
}
