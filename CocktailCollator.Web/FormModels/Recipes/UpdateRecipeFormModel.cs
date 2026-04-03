using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using CocktailCollator.Web.ViewModels.Measurements;

namespace CocktailCollator.Web.FormModels.Recipes;

public class UpdateRecipeFormModel(IMapper mapper) : IFormModel<UpdateRecipeInputPort>
{
    public InputProperty<List<UpdateRecipeFormModelIngredient>> Ingredients { get; set; } 
        = new(() => [], ValidateIngredients);
    public InputProperty<string> Name { get; set; } 
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<Guid> RecipeId { get; set; } 
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public InputProperty<List<UpdateRecipeFormModelStep>> Steps { get; set; } 
        = new(() => [], (inputList) => inputList.All(step => step.Instruction.IsValid() && step.Order.IsValid()));

    public UpdateRecipeInputPort ExtractToInputPort()
        => mapper.Map<UpdateRecipeInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid() && this.Steps.IsValid() && this.Ingredients.IsValid();

    public void ResetToDefault()
    {
        this.RecipeId.ResetToDefault();
        this.Name.ResetToDefault();
        this.Ingredients.ResetToDefault();
        this.Steps.ResetToDefault();
    }

    private static bool ValidateIngredients(List<UpdateRecipeFormModelIngredient> ingredients)
    {
        return ingredients.All(ingredient => ingredient.Name.IsValid()
            && ingredient.Amount.IsValid()
            && ingredient.Measurement.IsValid());
    }
}

public class UpdateRecipeFormModelIngredient
{
    public InputProperty<decimal> Amount { get; set; }
        = new(() => 1m, (input) => true);
    public Guid ExistingIngredientId { get; set; }
    public InputProperty<Guid> Measurement { get; set; }
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public MeasurementViewModel? MeasurementModel { get; set; }
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<bool> UsingExistingIngredient { get; set; }
        = new(() => true, (_) => true);
}

public class UpdateRecipeFormModelStep
{
    public InputProperty<string> Instruction { get; set; } 
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<int> Order { get; set; } 
        = new(() => 0, (input) => true);
}
