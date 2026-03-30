using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.GetIngredients;

public class GetIngredientsInteractor(ICocktailDbContext dbContext)
{
    public Task Interact(IGetIngredientsOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Ingredients = dbContext.GetEntities<Ingredient>()
            .Select(i => new Ingredient()
            {
                IngredientId = i.IngredientId,
                Name = i.Name,
                Measurements = i.Measurements!
                    .Select(im => new IngredientMeasurement
                    {
                        Ingredient = im.Ingredient,
                        IngredientId = im.IngredientId,
                        Measurement = im.Measurement,
                        MeasurementId = im.MeasurementId,
                    })
                    .ToList(),
                IngredientCategoryId = i.IngredientCategoryId,
                Category = i.Category,
            });

        return outputPort.Success([.. _Ingredients], cancellationToken);
    }
}
