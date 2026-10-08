using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;

public class DeleteRecipeCategoryInputPort : IInputPort<IDeleteRecipeCategoryOutputPort>
{
    public required Guid RecipeCategoryId { get; set; }
}
