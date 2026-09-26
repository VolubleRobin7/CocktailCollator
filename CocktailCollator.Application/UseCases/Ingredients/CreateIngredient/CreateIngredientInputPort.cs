using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;

public class CreateIngredientInputPort : IInputPort<ICreateIngredientOutputPort>
{
    public required string Name { get; set; }
}
