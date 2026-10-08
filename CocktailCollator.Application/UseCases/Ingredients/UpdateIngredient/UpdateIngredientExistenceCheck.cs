using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Application.Common.Pipes;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;

public class UpdateIngredientExistenceCheck(ICocktailDbContext dbContext)
    : ExistencePipeBase<UpdateIngredientInputPort, IUpdateIngredientOutputPort, Ingredient>(
        dbContext,
        inputPort => inputPort.IngredientId);
