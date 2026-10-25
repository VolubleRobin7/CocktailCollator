using CocktailCollator.Web.Common.Inputs;
using CocktailCollator.Web.ViewModels.Measurements;

namespace CocktailCollator.Web.FormModels.Recipes;

public class RecipeFormModelIngredient
{
    public InputProperty<decimal> Amount { get; set; }
        = new(() => 1m, ValidateAmount);
    public Guid ExistingIngredientId { get; set; }
    public InputProperty<Guid> Measurement { get; set; }
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public MeasurementViewModel? MeasurementModel { get; set; }
    public InputProperty<string> Name { get; set; }
    public bool UsingExistingIngredient { get; set; } = true;

    // This constructor is used by the ConfigureRecipeIngredientModal, which checks the new ingredient against the recipe's current ingredients.
    public RecipeFormModelIngredient(Func<string, ValidationResult>? nameValidationFunc = null)
    {
        this.Name = new(() => string.Empty, nameValidationFunc ?? ValidateName);
    }

    public void CopyTo(RecipeFormModelIngredient target)
    {
        target.Amount.Input = this.Amount.Input;
        target.ExistingIngredientId = this.ExistingIngredientId;
        target.Measurement.Input = this.Measurement.Input;
        target.MeasurementModel = this.MeasurementModel;
        target.Name.Input = this.Name.Input;
        target.UsingExistingIngredient = this.UsingExistingIngredient;
    }

    private static ValidationResult ValidateAmount(decimal input)
        => input switch
        {
            <= 0 => new(false, "Amount must be greater than zero."),
            { Scale: > 4 } => new(false, "Amount cannot have more than 4 decimal places."),
            _ => new(true)
        };

    private static ValidationResult ValidateName(string input)
        => string.IsNullOrWhiteSpace(input)
            ? new(false, "Ingredient name is required.")
            : new(true);
}

public class RecipeFormModelStep
{
    public InputProperty<string> Instruction { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<int> Order { get; set; }
        = new(() => 0, (input) => true);
}
