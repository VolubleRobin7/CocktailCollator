using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using CocktailCollator.Web.ViewModels.Measurements;
using System.Collections.ObjectModel;

namespace CocktailCollator.Web.FormModels.Recipes;

public class UpdateRecipeFormModel : IFormModel<UpdateRecipeInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<ObservableCollection<UpdateRecipeFormModelIngredient>> Ingredients { get; set; } 
        = new(() => [], ValidateIngredients);
    public InputProperty<string> Name { get; set; } 
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<Guid> RecipeId { get; set; } 
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public InputProperty<ObservableCollection<UpdateRecipeFormModelStep>> Steps { get; set; } 
        = new(() => [], (inputList) => inputList.All(step => step.Instruction.IsValid() && step.Order.IsValid()));

    public Action? OnChange { get; set; }

    public UpdateRecipeFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Ingredients.Input.CollectionChanged += (_, args) =>
        {
            if (args.NewItems is not null)
            {
                foreach (UpdateRecipeFormModelIngredient ingredient in args.NewItems)
                {
                    ingredient.Amount.OnChange = () => OnChange?.Invoke();
                    ingredient.Measurement.OnChange = () => OnChange?.Invoke();
                    ingredient.Name.OnChange = () => OnChange?.Invoke();
                }
            }
        };

        this.Name.OnChange = () => OnChange?.Invoke();

        this.Steps.Input.CollectionChanged += (_, args) =>
        {
            if (args.NewItems is not null)
            {
                foreach (UpdateRecipeFormModelStep step in args.NewItems)
                {
                    step.Instruction.OnChange = () => OnChange?.Invoke();
                    step.Order.OnChange = () => OnChange?.Invoke();
                }
            }
        };
    }

    public UpdateRecipeInputPort ExtractToInputPort()
        => this._mapper.Map<UpdateRecipeInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid() && this.Steps.IsValid() && this.Ingredients.IsValid();

    public void ResetToDefault()
    {
        this.RecipeId.ResetToDefault();
        this.Name.ResetToDefault();
        this.Ingredients.ResetToDefault();
        this.Steps.ResetToDefault();
    }

    private static bool ValidateIngredients(ObservableCollection<UpdateRecipeFormModelIngredient> ingredients)
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
