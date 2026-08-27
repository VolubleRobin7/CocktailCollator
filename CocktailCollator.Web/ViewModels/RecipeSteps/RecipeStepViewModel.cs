using CocktailCollator.Web.Common.State;

namespace CocktailCollator.Web.ViewModels.RecipeSteps;

public class RecipeStepViewModel : IStoreableViewModel<RecipeStepViewModel>
{
    public string? Instruction { get; set; }
    public int? Order { get; set; }
    public required Guid RecipeStepId { get; set; }

    public void ApplyChanges(RecipeStepViewModel source, IViewModelStore store)
    {
        this.Instruction = source.Instruction;
        this.Order = source.Order;
    }
}
