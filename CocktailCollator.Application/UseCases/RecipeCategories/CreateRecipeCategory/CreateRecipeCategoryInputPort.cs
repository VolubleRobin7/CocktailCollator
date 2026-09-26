using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;

public class CreateRecipeCategoryInputPort : IInputPort<ICreateRecipeCategoryOutputPort>
{
    public required string Name { get; set; }
}
