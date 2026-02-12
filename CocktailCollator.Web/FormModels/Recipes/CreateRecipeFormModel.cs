using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.Recipes;

public class CreateRecipeFormModel(IMapper mapper) : IFormModel<CreateRecipeInputPort>
{
    public InputProperty<List<CreateRecipeFormModelIngredient>> Ingredients { get; set; }
        = new(() => [], ValidateIngredients);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<List<CreateRecipeFormModelStep>> Steps { get; set; }
        = new(() => [], (inputList) => inputList.All(step => step.Instruction.IsValid() && step.Order.IsValid()));

    public CreateRecipeInputPort ExtractToInputPort()
        => mapper.Map<CreateRecipeInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid() && this.Steps.IsValid() && this.Ingredients.IsValid();

    public void ResetToDefault()
    {
        this.Name.ResetToDefault();
        this.Steps.ResetToDefault();
        this.Ingredients.ResetToDefault();
    }

    private static bool ValidateIngredients(List<CreateRecipeFormModelIngredient> ingredients)
    {
        return ingredients.All(ingredient => ingredient.Name.IsValid()
            && ingredient.Amount.IsValid()
            && ingredient.Measurement.IsValid());
    }
}

public class CreateRecipeFormModelIngredient
{
    public InputProperty<decimal> Amount { get; set; }
        = new(() => 0m, (input) => true);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<CreateRecipeFormModelMeasurement> Measurement { get; set; }
        = new(() => new(), (input) => input.Name.IsValid());
}

public class CreateRecipeFormModelMeasurement
{
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input) && input.Length <= 20);
}

public class CreateRecipeFormModelStep
{
    public InputProperty<string> Instruction { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<int> Order { get; set; }
        = new(() => 0, (input) => true);
}
