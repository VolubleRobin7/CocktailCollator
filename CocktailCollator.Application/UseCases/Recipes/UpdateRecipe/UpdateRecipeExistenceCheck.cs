using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Application.Common.Pipes;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeExistenceCheck(ICocktailDbContext dbContext)
    : ExistencePipeBase<UpdateRecipeInputPort, IUpdateRecipeOutputPort, Recipe>(
        dbContext,
        inputPort => inputPort.RecipeId);
