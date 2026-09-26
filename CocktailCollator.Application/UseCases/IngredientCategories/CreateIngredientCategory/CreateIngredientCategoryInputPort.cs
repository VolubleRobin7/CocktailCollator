using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;

public class CreateIngredientCategoryInputPort : IInputPort<ICreateIngredientCategoryOutputPort>
{
    public required string Name { get; set; }
}
