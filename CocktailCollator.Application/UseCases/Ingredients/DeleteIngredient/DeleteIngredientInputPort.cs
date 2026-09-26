using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;

public class DeleteIngredientInputPort : IInputPort<IDeleteIngredientOutputPort>
{
    public required Guid IngredientId { get; set; }
}
