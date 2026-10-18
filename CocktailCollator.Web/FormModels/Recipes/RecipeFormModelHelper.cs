using CocktailCollator.Web.Common.Inputs;
using CocktailCollator.Web.ViewModels.Measurements;

namespace CocktailCollator.Web.FormModels.Recipes;

public class RecipeFormModelIngredient
{
    public InputProperty<decimal> Amount { get; set; }
        = new(() => 1m, (input) => true);
    public Guid ExistingIngredientId { get; set; }
    public InputProperty<Guid> Measurement { get; set; }
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public MeasurementViewModel? MeasurementModel { get; set; }
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public bool UsingExistingIngredient { get; set; } = true;

    public void CopyTo(RecipeFormModelIngredient target)
    {
        target.Amount.Input = this.Amount.Input;
        target.ExistingIngredientId = this.ExistingIngredientId;
        target.Measurement.Input = this.Measurement.Input;
        target.MeasurementModel = this.MeasurementModel;
        target.Name.Input = this.Name.Input;
        target.UsingExistingIngredient = this.UsingExistingIngredient;
    }
}

public class RecipeFormModelStep
{
    public InputProperty<string> Instruction { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<int> Order { get; set; }
        = new(() => 0, (input) => true);
}
