using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.Recipes;

public class CreateRecipeFormModel(IMapper mapper) : IFormModel<CreateRecipeInputPort>
{
    public InputProperty<List<CreateRecipeFormModelIngredient>> Ingredients { get; set; }
        = new(() => [], (inputList) => inputList.All(ingredient => ingredient.Name.IsValid()));
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
}

public class CreateRecipeFormModelIngredient
{
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
}

public class CreateRecipeFormModelStep
{
    public InputProperty<string> Instruction { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<int> Order { get; set; }
        = new(() => 0, (input) => true);
}
