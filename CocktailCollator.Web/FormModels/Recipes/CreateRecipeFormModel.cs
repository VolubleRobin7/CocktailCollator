using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using CocktailCollator.Web.ViewModels.Measurements;
using System.Collections.ObjectModel;

namespace CocktailCollator.Web.FormModels.Recipes;

public class CreateRecipeFormModel : IFormModel<CreateRecipeInputPort>
{
    private readonly IMapper _mapper;

    // I'm now more thinking that these should be more like List<InputProp<object...
    // That would allow more precise control, could set better validation, and reset per item
    // Or possibly even InputProp<List<InputProp<object... but that could be overkill
    // Could require the creation of a new class that wraps List, something like InputPropertyList<T>
    public InputProperty<ObservableCollection<CreateRecipeFormModelIngredient>> Ingredients { get; set; }
        = new(() => [], ValidateIngredients);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<ObservableCollection<CreateRecipeFormModelStep>> Steps { get; set; }
        = new(() => [], (inputList) => inputList.All(step => step.Instruction.IsValid() && step.Order.IsValid()));

    public Action? OnChange { get; set; }

    public CreateRecipeFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Ingredients.Input.CollectionChanged += (_, args) =>
        {
            if (args.NewItems is not null)
            {
                foreach (CreateRecipeFormModelIngredient ingredient in args.NewItems)
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
                foreach (CreateRecipeFormModelStep step in args.NewItems)
                {
                    step.Instruction.OnChange = () => OnChange?.Invoke();
                    step.Order.OnChange = () => OnChange?.Invoke();
                }
            }
        };
    }

    public CreateRecipeInputPort ExtractToInputPort()
        => this._mapper.Map<CreateRecipeInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid() && this.Steps.IsValid() && this.Ingredients.IsValid();

    public void ResetToDefault()
    {
        this.Name.ResetToDefault();
        this.Steps.ResetToDefault();
        this.Ingredients.ResetToDefault();
    }

    private static bool ValidateIngredients(ObservableCollection<CreateRecipeFormModelIngredient> ingredients)
    {
        return ingredients.All(ingredient => ingredient.Name.IsValid()
            && ingredient.Amount.IsValid()
            && ingredient.Measurement.IsValid());
    }
}

public class CreateRecipeFormModelIngredient
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

public class CreateRecipeFormModelStep
{
    public InputProperty<string> Instruction { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<int> Order { get; set; }
        = new(() => 0, (input) => true);
}
