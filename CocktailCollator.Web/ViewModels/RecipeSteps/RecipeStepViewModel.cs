namespace CocktailCollator.Web.ViewModels.RecipeSteps;

public class RecipeStepViewModel
{
    public string? Instruction { get; set; }
    public int? Order { get; set; }
    public required Guid RecipeStepId { get; set; }
}
