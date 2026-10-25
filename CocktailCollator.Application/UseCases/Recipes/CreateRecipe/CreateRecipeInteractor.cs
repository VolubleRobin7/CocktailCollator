using AutoMapper;
using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;
using System.Diagnostics.CodeAnalysis;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeInteractor(ICocktailDbContext dbContext, IMapper mapper) : IInteractorPipe<CreateRecipeInputPort, ICreateRecipeOutputPort>
{
    public async Task ExecuteAsync(CreateRecipeInputPort inputPort, ICreateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        if (!this.PassesRules(inputPort, out var _FailureReason))
        {
            await outputPort.RuleViolation(_FailureReason, cancellationToken);
            return;
        }

        var _Recipe = mapper.Map<Recipe>(inputPort);

        dbContext.Add(_Recipe);

        foreach (var _Image in inputPort.Images)
        {
            var _DocumentId = dbContext.QueueAddDocument(_Image, _Recipe);

            var recipeImage = new RecipeDocument
            {
                RecipeId = _Recipe.RecipeId,
                DocumentId = _DocumentId,
            };

            _Recipe.Images ??= [];
            _Recipe.Images.Add(recipeImage);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Recipe, cancellationToken);
    }

    private bool PassesRules(CreateRecipeInputPort inputPort, [NotNullWhen(false)] out string? failureReason)
    {
        if (inputPort.Ingredients.Any(i => i.Amount <= 0))
        {
            failureReason = "Ingredient amounts must be greater than zero.";
            return false;
        }

        var _ExistingIngredientIds = inputPort.Ingredients
            .Where(i => i.Ingredient is null)
            .Select(i => i.IngredientId)
            .ToList();

        if (_ExistingIngredientIds.Count != _ExistingIngredientIds.Distinct().Count())
        {
            failureReason = "Cannot add the same ingredient more than once.";
            return false;
        }

        var _ExistingIngredients = dbContext.GetEntities<Ingredient>()
            .Where(i => _ExistingIngredientIds.Contains(i.IngredientId))
            .Select(i => new
            {
                i.IngredientId,
                i.Name,
                MeasurementIds = i.Measurements != null
                    ? i.Measurements.Select(m => m.MeasurementId).ToList()
                    : new List<Guid>()
            })
            .ToDictionary(i => i.IngredientId);

        foreach (var _RecipeIngredient in inputPort.Ingredients.Where(i => i.Ingredient is null))
        {
            if (!_ExistingIngredients.TryGetValue(_RecipeIngredient.IngredientId, out var _ExistingIngredient)
                || !_ExistingIngredient.MeasurementIds.Contains(_RecipeIngredient.MeasurementId))
            {
                failureReason = "Selected measurement is not associated with the ingredient.";
                return false;
            }
        }

        var _AllIngredientNames = _ExistingIngredients.Values
            .Select(i => i.Name.Trim())
            .Concat(inputPort.Ingredients
                .Where(i => i.Ingredient is not null)
                .Select(i => i.Ingredient!.Name.Trim()))
            .ToList();

        if (_AllIngredientNames.Count != _AllIngredientNames.Distinct(StringComparer.OrdinalIgnoreCase).Count())
        {
            failureReason = "Cannot add the same ingredient more than once.";
            return false;
        }

        failureReason = null;
        return true;
    }
}
