using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;

public class DeleteRecipeInputPort : IInputPort<IDeleteRecipeOutputPort>
{
    public required Guid RecipeId { get; set; }
}
