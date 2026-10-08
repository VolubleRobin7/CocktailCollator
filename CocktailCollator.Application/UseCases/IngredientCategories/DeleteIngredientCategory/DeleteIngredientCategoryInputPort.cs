using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.Application.UseCases.IngredientCategories.DeleteIngredientCategory;

public class DeleteIngredientCategoryInputPort : IInputPort<IDeleteIngredientCategoryOutputPort>
{
    public required Guid IngredientCategoryId { get; set; }
}
